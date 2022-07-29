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
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            GameInputModel[] dtoGames = JsonConvert.DeserializeObject<GameInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (GameInputModel dtoGame in dtoGames)
            {
                if (!IsValid(dtoGame) || !dtoGame.Tags.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Game newGame = new Game
                {
                    Name = dtoGame.Name,
                    Price = dtoGame.Price,
                    ReleaseDate = dtoGame.ReleaseDate,
                    Developer = context.Developers.FirstOrDefault(x => x.Name == dtoGame.Developer) ?? new Developer { Name = dtoGame.Developer },
                    Genre = context.Genres.FirstOrDefault(x => x.Name == dtoGame.Genre) ?? new Genre { Name = dtoGame.Genre }
                };

                foreach (string gameTag in dtoGame.Tags)
                {
                    Tag newGameTag = context.Tags.FirstOrDefault(x => x.Name == gameTag) ?? new Tag { Name = gameTag };
                    newGame.GameTags.Add(new GameTag { Tag = newGameTag });
                }
                context.Games.Add(newGame);
                context.SaveChanges();
                sb.AppendLine($"Added {newGame.Name} ({newGame.Genre.Name}) with {newGame.GameTags.Count} tags");

            }
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            UserInputModel[] dtoUsers = JsonConvert.DeserializeObject<UserInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<User> users = new List<User>();

            foreach (UserInputModel dtoUser in dtoUsers)
            {
                if (!IsValid(dtoUser) || !dtoUser.Cards.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                User newUser = new User
                {
                    FullName = dtoUser.FullName,
                    Username = dtoUser.Username,
                    Email = dtoUser.Email,
                    Age = dtoUser.Age,
                    Cards = dtoUser.Cards.Select(x => new Card
                    {
                        Number = x.Number,
                        Cvc = x.Cvc,
                        Type = Enum.Parse<CardType>(x.Type)
                    })
                    .ToArray()
                };
                users.Add(newUser);
                sb.AppendLine($"Imported {newUser.Username} with {newUser.Cards.Count} cards");

            }
            context.Users.AddRange(users);
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

            foreach (PurchaseInputModel dtoPurchase in dtoPurchases)
            {
                if (!IsValid(dtoPurchase))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                Purchase newPurchase = new Purchase
                {
                    Game = context.Games.FirstOrDefault(x => x.Name == dtoPurchase.GameName),
                    Type = dtoPurchase.Type.Value,
                    ProductKey = dtoPurchase.ProductKey,
                    Card = context.Cards.FirstOrDefault(x => x.Number == dtoPurchase.CardNumber),
                    Date = DateTime.ParseExact(dtoPurchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None)

                };
                context.Purchases.Add(newPurchase);
                context.SaveChanges();
                sb.AppendLine($"Imported {newPurchase.Game.Name} for {newPurchase.Card.User.Username}");
            }
            return sb.ToString().TrimEnd(); 

        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}