namespace Footballers
{
    using AutoMapper;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.DataProcessor.ImportDto;
    using System;
    using System.Globalization;
    using System.Linq;

    public class FootballersProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public FootballersProfile()
        {
            //Input

            CreateMap<CoachWithFootballersInputModel, Coach>()
               .ForMember(d => d.Footballers, mo => mo.Ignore());
            CreateMap<FootballerOfCoachInputModel, Footballer>()
                .ForMember(d => d.ContractStartDate, mo => mo.MapFrom(s => DateTime.ParseExact(s.ContractStartDate,
                "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(d => d.ContractEndDate, mo => mo.MapFrom(s => DateTime.ParseExact(s.ContractEndDate,
                "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(d => d.BestSkillType, mo => mo.MapFrom(s => Enum.Parse<BestSkillType>(s.BestSkillType)))
                .ForMember(d => d.PositionType, mo => mo.MapFrom(s => Enum.Parse<PositionType>(s.PositionType)));


            CreateMap<TeamInputModel, Team>()
                .ForMember(d => d.TeamsFootballers, mo => mo.Ignore());
            CreateMap<Footballer, TeamFootballer>()
                .ForMember(d => d.FootballerId, mo => mo.MapFrom(s => s.Id))
                .ForMember(d => d.Team, mo => mo.Ignore());


            //Output

            CreateMap<Footballer, FootballerOfCoachOutputModel>()
                .ForMember(d => d.FootballerName, mo => mo.MapFrom(s => s.Name))
                .ForMember(d => d.PositionType, mo => mo.MapFrom(s => s.PositionType.ToString()));
            CreateMap<Coach, CoachOutputModel>()
                .ForMember(d => d.FootballersCount, mo => mo.MapFrom(s => s.Footballers.Count))
                .ForMember(d => d.CoachName, mo => mo.MapFrom(s => s.Name))
                .ForMember(d => d.Footballers, mo => mo.MapFrom(s => s.Footballers.OrderBy(f=>f.Name)));


            CreateMap<TeamFootballer, FootballerOfTeamOutputModel>()                
                .ForMember(d => d.FootballerName, mo => mo.MapFrom(s => s.Footballer.Name))
                .ForMember(d => d.ContractStartDate, mo => mo.MapFrom(s => s.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture)))
                .ForMember(d => d.ContractEndDate, mo => mo.MapFrom(s => s.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture)))
                .ForMember(d => d.BestSkillType, mo => mo.MapFrom(s => s.Footballer.BestSkillType.ToString()))
                .ForMember(d => d.PositionType, mo => mo.MapFrom(s => s.Footballer.PositionType.ToString()));
          
        }
    }
}
