namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        static IMapper mapper;
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            GameInputModel[] dtoGames = JsonConvert.DeserializeObject<GameInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (GameInputModel dtoGame in dtoGames)
            {
                if (!IsValid(dtoGame) || !dtoGame.TagsNames.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                bool isValidReleaseDate = DateTime.TryParseExact(dtoGame.ReleaseDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);
                if (!isValidReleaseDate)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                Game newGame = new Game()
                {
                    Name = dtoGame.Name,
                    Price = dtoGame.Price,
                    ReleaseDate = releaseDate,
                    Developer = context.Developers.FirstOrDefault(x => x.Name == dtoGame.DeveloperName) ?? 
                        new Developer { Name = dtoGame.DeveloperName },
                    Genre = context.Genres.FirstOrDefault(x => x.Name == dtoGame.Genre) ?? 
                        new Genre { Name = dtoGame.Genre },
                };

                foreach (string tag in dtoGame.TagsNames)
                {
                    Tag newTag = context.Tags.FirstOrDefault(x => x.Name == tag) ?? new Tag { Name = tag };
                    newGame.GameTags.Add(new GameTag { Tag = newTag });
                }

                context.Games.Add(newGame);
                context.SaveChanges();
                sb.AppendLine($"Added {newGame.Name} ({newGame.Genre.Name}) with {newGame.GameTags.Count} tags");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            UserInputModel[] dtoUsers = JsonConvert.DeserializeObject<UserInputModel[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            List<User> validUsers = new List<User>();

            foreach (UserInputModel dtoUser in dtoUsers)
            {
                if (!IsValid(dtoUser) || !dtoUser.Cards.Any() || !dtoUser.Cards.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                InitializeAutoMapper();
                User newUser = mapper.Map<User>(dtoUser);
                validUsers.Add(newUser);
                sb.AppendLine($"Imported {newUser.Username} with {newUser.Cards.Count} cards");
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Purchases");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PurchaseInputModel[]), xmlRootAttribute);

            using StringReader reader = new StringReader(xmlString);

            PurchaseInputModel[] dtoPurchases = (PurchaseInputModel[])xmlSerializer.Deserialize(reader);
            StringBuilder sb = new StringBuilder();

            List<Purchase> validPurchases = new List<Purchase>();

            foreach (PurchaseInputModel dtoPurchase in dtoPurchases)
            {
                if (!IsValid(dtoPurchase))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Purchase newPurchase = new Purchase()
                {
                    Type = Enum.Parse<PurchaseType>(dtoPurchase.Type),
                    ProductKey = dtoPurchase.ProductKey,
                    Date = DateTime.ParseExact(dtoPurchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Card = context.Cards.FirstOrDefault(x => x.Number == dtoPurchase.CardNumber),
                    Game = context.Games.FirstOrDefault(x => x.Name == dtoPurchase.GameName)

                };

                validPurchases.Add(newPurchase);
                sb.AppendLine($"Imported {newPurchase.Game.Name} for {newPurchase.Card.User.Username}");

            }
            context.Purchases.AddRange(validPurchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static void InitializeAutoMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<VaporStoreProfile>());

            mapper = mapperConfiguration.CreateMapper();
        }
    }
}