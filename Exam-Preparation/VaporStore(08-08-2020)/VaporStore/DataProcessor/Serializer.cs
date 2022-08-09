namespace VaporStore.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
        static IMapper mapper;
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            //var dtoGenres = context.Genres.Where(x => genreNames.Contains(x.Name))
            //.ToArray()
            //.Select(x => new
            //{
            //    Id = x.Id,
            //    Genre = x.Name,
            //    Games = x.Games.Where(g => g.Purchases.Any()).Select(g => new
            //    {
            //        Id = g.Id,
            //        Title = g.Name,
            //        Developer = g.Developer.Name,
            //        Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name)),
            //        Players = g.Purchases.Count

            //    })
            //    .OrderByDescending(x => x.Players)
            //    .ThenBy(x => x.Id)
            //    .ToArray(),
            //    TotalPlayers = x.Games.Where(g => g.Purchases.Any()).Sum(g => g.Purchases.Count)
            //})
            //.OrderByDescending(x => x.TotalPlayers)
            //.ThenBy(x => x.Id)
            //.ToArray();

            //With AutoMapper


            GenreOutputModel[] dtoGenres = context.Genres
                .Where(x => genreNames.Contains(x.Name))
                .ToList()
                .AsQueryable()
                .ProjectTo<GenreOutputModel>(InitializeAutoMapperConfig())
                .OrderByDescending(x => x.TotalPurchaseCount)
                .ThenBy(x => x.Id)
                .ToArray();

            return JsonConvert.SerializeObject(dtoGenres, Formatting.Indented);
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
            //UserPurchaseOutputModel[] dtoPurchases = context.Users
            //    .Include(x=>x.Cards)
            //    .ToArray()
            //    .Where(x => x.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == storeType)))
            //    .Select(x => new UserPurchaseOutputModel
            //    {
            //        Username = x.Username,
            //        Purchases = x.Cards.SelectMany(c => c.Purchases).Where(p => p.Type.ToString() == storeType)
            //        .Select(p => new PurchaseOutputModel
            //        {
            //            CardNumber = p.Card.Number,
            //            Cvc = p.Card.Cvc,
            //            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            //            Game = new GameOfPurchaseOutputModel
            //            {
            //                Name = p.Game.Name,
            //                Genre = p.Game.Genre.Name,
            //                Price = p.Game.Price
            //            }
            //        })
            //        .OrderBy(p => p.Date)
            //        .ToArray(),
            //        TotalSpent = x.Cards.Sum(c => c.Purchases.Where(p => p.Type.ToString() == storeType).Sum(p => p.Game.Price))

            //    })
            //    .OrderByDescending(x => x.TotalSpent)
            //    .ThenBy(x => x.Username)
            //    .ToArray();

            UserPurchaseOutputModel[] dtoPurchases = context.Users
                .Include(x=>x.Cards)
                .ToArray()
                .Where(x=>x.Cards.Any(c=>c.Purchases.Any(p=>p.Type.ToString() == storeType)))
                .Select(x=> new UserPurchaseOutputModel
                {
                    Username = x.Username,
                    Purchases = x.Cards.SelectMany(c=>c.Purchases).Where(p=>p.Type.ToString() == storeType)
                    .OrderBy(p=>p.Date)
                    .AsQueryable()
                    .ProjectTo<PurchaseOutputModel>(InitializeAutoMapperConfig())
                    .ToArray(),
                    TotalSpent = x.Cards.Sum(c=>c.Purchases.Where(p=>p.Type.ToString() == storeType).Sum(p=>p.Game.Price))
                    
                })
                .OrderByDescending(x=>x.TotalSpent)
                .ThenBy(x=>x.Username)
                .ToArray();


            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Users");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserPurchaseOutputModel[]), xmlRootAttribute);

            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            xmlSerializer.Serialize(writer, dtoPurchases, xmlSerializerNamespaces);

            return writer.ToString();


        }
       private static MapperConfiguration InitializeAutoMapperConfig()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            cfg.AddProfile<VaporStoreProfile>());

            mapper = config.CreateMapper();
            return config;

        }
    }
}