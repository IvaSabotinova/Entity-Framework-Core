namespace Footballers.DataProcessor
{
    using AutoMapper;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;


    public class Deserializer
    {
        static IMapper mapper;
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Coaches");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CoachWithFootballersInputModel[]), xmlRootAttribute);

            using StringReader reader = new StringReader(xmlString);

            CoachWithFootballersInputModel[] dtoCoaches = (CoachWithFootballersInputModel[])xmlSerializer.Deserialize(reader);

            StringBuilder sb = new StringBuilder();
            List<Coach> validCoaches = new List<Coach>();

            foreach (CoachWithFootballersInputModel dtoCoach in dtoCoaches)
            {
                if (!IsValid(dtoCoach) || String.IsNullOrEmpty(dtoCoach.Nationality) || DateTime.TryParse(dtoCoach.Nationality, out DateTime dt) == true)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Coach newCoach = new Coach()
                //{
                //    Name = dtoCoach.Name,
                //    Nationality = dtoCoach.Nationality,

                //};

                //With AutoMapper

                 //Coach newCoach = Mapper.Map<Coach>(dtoCoach);

                InitializeAutoMapper();

                Coach newCoach = mapper.Map<Coach>(dtoCoach);


                List<Footballer> validFootballers = new List<Footballer>();
                foreach (FootballerOfCoachInputModel dtoFootballer in dtoCoach.Footballers)
                {
                    if (!IsValid(dtoFootballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool IsValidContractStartDate = DateTime.TryParseExact(dtoFootballer.ContractStartDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractStarDate);

                    bool IsValidContractEndDate = DateTime.TryParseExact(dtoFootballer.ContractEndDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractEndDate);

                    if (!IsValidContractStartDate || !IsValidContractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (contractStarDate > contractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    //Footballer newFootballer = new Footballer()
                    //{
                    //    Name = dtoFootballer.Name,
                    //    ContractStartDate = DateTime.ParseExact(dtoFootballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    //    ContractEndDate = DateTime.ParseExact(dtoFootballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    //    BestSkillType = Enum.Parse<BestSkillType>(dtoFootballer.BestSkillType),
                    //    PositionType = Enum.Parse<PositionType>(dtoFootballer.PositionType)
                    //};

                    //With AutoMapper 

                    //Footballer newFootballer = Mapper.Map<Footballer>(dtoFootballer);

                    InitializeAutoMapper();
                    Footballer newFootballer = mapper.Map<Footballer>(dtoFootballer);

                    validFootballers.Add(newFootballer);

                }
                newCoach.Footballers = validFootballers;
                validCoaches.Add(newCoach);
                sb.AppendLine(String.Format(SuccessfullyImportedCoach, newCoach.Name, newCoach.Footballers.Count));
            }
            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            TeamInputModel[] dtoTeams = JsonConvert.DeserializeObject<TeamInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Team> validTeams = new List<Team>();

            foreach (TeamInputModel dtoTeam in dtoTeams)
            {
                if (!IsValid(dtoTeam) || dtoTeam.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Team newTeam = new Team
                //{
                //    Name = dtoTeam.Name,
                //    Nationality = dtoTeam.Nationality,
                //    Trophies = dtoTeam.Trophies
                //};

                //With AutoMapper

                //Team newTeam = Mapper.Map<Team>(dtoTeam);

                InitializeAutoMapper();
                Team newTeam = mapper.Map<Team>(dtoTeam);


                List<int> validFootballersIds = context.Footballers.Select(f => f.Id).ToList();
              

                foreach (int dtoFootballerId in dtoTeam.Footballers.Distinct())
                {
                    //if (!validFootballersIds.Contains(dtoFootballerId))
                    //{
                    //    sb.AppendLine(ErrorMessage);
                    //    continue;
                    //}

                    //newTeam.TeamsFootballers.Add(new TeamFootballer
                    //{
                    //    FootballerId = dtoFootballerId,
                    //    Team = newTeam
                    //});

                    //With AutoMapper

                    Footballer footballer = context.Footballers.Find(dtoFootballerId);

                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    //TeamFootballer teamFootballer = Mapper.Map<TeamFootballer>(footballer);

                    TeamFootballer teamFootballer = mapper.Map<TeamFootballer>(footballer);
                    teamFootballer.Team = newTeam;

                    newTeam.TeamsFootballers.Add(teamFootballer);
                }

                validTeams.Add(newTeam);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, newTeam.Name, newTeam.TeamsFootballers.Count));

            }
            context.AddRange(validTeams);
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
            MapperConfiguration config = new MapperConfiguration(cfg =>
            cfg.AddProfile<FootballersProfile>());

            mapper = config.CreateMapper();
        }
    }
}
