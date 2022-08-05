namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ExportDto;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Globalization;
    using System.Linq;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            //Input

            CreateMap<CellInputModel, Cell>();
            CreateMap<DepartmentCellInputModel, Department>()
                .ForMember(d => d.Cells, mo => mo.MapFrom(s => s.Cells));


            CreateMap<MailOfPrisonerInputModel, Mail>();
            CreateMap<PrisonerMailInputModel, Prisoner>()
                .ForMember(d => d.Mails, mo => mo.MapFrom(s => s.Mails))
                .ForMember(d => d.IncarcerationDate, mo => mo.MapFrom(s => DateTime.ParseExact(s.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(d => d.ReleaseDate, mo => mo.Ignore());


            CreateMap<PrisonerOfOfficerInputModel, OfficerPrisoner>()
                .ForMember(d => d.PrisonerId, mo => mo.MapFrom(s => s.Id));
            CreateMap<OfficerPrisonerInputModel, Officer>()
                .ForMember(d => d.Position, mo => mo.MapFrom(s => Enum.Parse<Position>(s.Position)))
                .ForMember(d => d.Weapon, mo => mo.MapFrom(s => Enum.Parse<Weapon>(s.Weapon)))
                .ForMember(d => d.OfficerPrisoners, mo => mo.MapFrom(s => s.Prisoners));

            //Ouput

            CreateMap<OfficerPrisoner, OfficerOfPrisonerOutputModel>()
                .ForMember(d => d.FullName, mo => mo.MapFrom(s => s.Officer.FullName))
                .ForMember(d => d.DepartmentName, mo => mo.MapFrom(s => s.Officer.Department.Name));
            CreateMap<Prisoner, PrisonerWithOfficersOutputModel>()
                .ForMember(d => d.FullName, mo => mo.MapFrom(s => s.FullName))
                .ForMember(d => d.CellNumber, mo => mo.MapFrom(s => s.Cell.CellNumber))
                .ForMember(d => d.Officers, mo => mo.MapFrom(s => s.PrisonerOfficers.OrderBy(po=>po.Officer.FullName)))
                .ForMember(d => d.TotalOfficerSalary, mo => mo.MapFrom(s => decimal.Parse(
                s.PrisonerOfficers.Sum(po => po.Officer.Salary).ToString("f2"))));


            CreateMap<Mail, MailOfPrisonerOutputModel>()
                .ForMember(d => d.Description, mo => mo.MapFrom(s => string.Join("", s.Description.Reverse())));
            CreateMap<Prisoner, PrisonerOutputModel>()
                .ForMember(d => d.IncarcerationDate, mo => mo.MapFrom(s => s.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)))
                .ForMember(d => d.Mails, mo => mo.MapFrom(s => s.Mails));
        }
    }
}
