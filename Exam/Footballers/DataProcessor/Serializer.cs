namespace Footballers.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            //CoachOutputModel[] dtoCoaches = context.Coaches.ToArray().Where(x => x.Footballers.Any())
            //   .Select(x => new CoachOutputModel
            //   {
            //       FootballersCount = x.Footballers.Count,
            //       CoachName = x.Name,
            //       Footballers = x.Footballers.Select(f => new FootballerOfCoachOutputModel
            //       {
            //           FootballerName = f.Name,
            //           PositionType = f.PositionType.ToString()
            //       })
            //        .OrderBy(x => x.FootballerName)
            //        .ToArray()
            //   })
            //    .OrderByDescending(x => x.FootballersCount)
            //    .ThenBy(x => x.CoachName)
            //    .ToArray();

            CoachOutputModel[] dtoCoaches = context.Coaches
                .ToArray()
                .Where(x=>x.Footballers.Any())
                .AsQueryable()
                .ProjectTo<CoachOutputModel>(InitializeAutoMapperConfig())
                .OrderByDescending(x=>x.FootballersCount)
                .ThenBy(x=>x.CoachName)
                .ToArray(); 

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Coaches");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CoachOutputModel[]), xmlRootAttribute);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            
             xmlSerializer.Serialize(writer, dtoCoaches, namespaces);

            
            return sb.ToString().TrimEnd();

           
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {

            //var teams = context.Teams.ToArray().Where(x => x.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date)).Select(x => new
            //{
            //    Name = x.Name,
            //    Footballers = x.TeamsFootballers
            //    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
            //    .ThenBy(tf => tf.Footballer.Name)
            //    .Where(tf => tf.Footballer.ContractStartDate >= date)
            //    .Select(f => new
            //    {
            //        FootballerName = f.Footballer.Name,
            //        ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
            //        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
            //        BestSkillType = f.Footballer.BestSkillType.ToString(),
            //        PositionType = f.Footballer.PositionType.ToString()

            //    })
            //        .ToArray()
            //})
            //    .OrderByDescending(x => x.Footballers.Count())
            //    .ThenBy(x => x.Name)
            //    .Take(5)
            //    .ToArray();


            TeamWithFootballersOutputModel[] dtoTeams = context.Teams
                .ToArray()
                .Where(x => x.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .Select(x => new TeamWithFootballersOutputModel
                {
                    Name = x.Name,
                    Footballers = x.TeamsFootballers
                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                    .ThenBy(tf => tf.Footballer.Name)
                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                    .AsQueryable()
                    .ProjectTo<FootballerOfTeamOutputModel>(InitializeAutoMapperConfig())
                    .ToArray()
                })
                .OrderByDescending(x => x.Footballers.Count())
                .ThenBy(x => x.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(dtoTeams, Formatting.Indented);
        }

        private static MapperConfiguration InitializeAutoMapperConfig()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            cfg.AddProfile<FootballersProfile>());

            IMapper mapper = config.CreateMapper();
            return config;

        }
    }
}
