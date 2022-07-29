namespace VaporStore.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            GenreWithGamesOutputModel[] dtoGenres = context.Genres
                .Where(x => genreNames.Contains(x.Name))
                .ToList() //not needed, added for judge
                .Select(x => new GenreWithGamesOutputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Games = x.Games.Where(g => g.Purchases.Any())
                    .Select(g => new GameOutputModel
                    {
                        Id = g.Id,
                        Name = g.Name,
                        DeveloperName = g.Developer.Name,
                        TagsNames = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name)),
                        PurchaseCount = g.Purchases.Count

                    })
                    .OrderByDescending(x => x.PurchaseCount)
                    .ThenBy(x => x.Id)
                    .ToArray(),
                    TotalPlayers = x.Games.Where(g => g.Purchases.Any()).Sum(g => g.Purchases.Count())

                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToArray();

            return JsonConvert.SerializeObject(dtoGenres, Formatting.Indented);
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            PurchaseType inputType = Enum.Parse<PurchaseType>(storeType);

            UserWithPurchasesOutputModel[] users = context.Users
                .ToArray()
                .Where(x => x.Cards.Any(c => c.Purchases.Count(y=>y.Type == inputType) > 0))
                .Select(x => new UserWithPurchasesOutputModel
                {
                    Username = x.Username,
                    MoneySpent = x.Cards.Sum(c => c.Purchases.Where(p=>p.Type == inputType).Sum(p => p.Game.Price)),
                    Purchases = x.Cards.SelectMany(c => c.Purchases)
                    .Where(p => p.Type == inputType).Select(p => new PurchaseOutputModel
                    {
                        CardNumber = p.Card.Number,
                        Cvc = p.Card.Cvc,
                        DateTime = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        XmlGame = new XmlGameOutputModel
                        {
                            GameName = p.Game.Name,
                            GenreName = p.Game.Genre.Name,
                            Price = p.Game.Price
                        },

                    })
                    .OrderBy(x => x.DateTime)
                    .ToArray()
                })
                .OrderByDescending(x=>x.MoneySpent)
                .ThenBy(x=>x.Username)
                .ToArray();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Users"); 

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserWithPurchasesOutputModel[]), xmlRootAttribute);

            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            xmlSerializer.Serialize(writer, users, xmlSerializerNamespaces);

            return writer.ToString();   

        }
    }
}