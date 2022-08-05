namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
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
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            DepartmentCellInputModel[] dtoDepartments = JsonConvert.DeserializeObject<DepartmentCellInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Department> validDepartments = new List<Department>();

            foreach (DepartmentCellInputModel dtoDepartment in dtoDepartments)
            {
                if(!IsValid(dtoDepartment) || !dtoDepartment.Cells.All(IsValid) || !dtoDepartment.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                InitializeAutoMapper();
                Department newDepartment = mapper.Map<Department>(dtoDepartment);
                validDepartments.Add(newDepartment);
                sb.AppendLine($"Imported {newDepartment.Name} with {newDepartment.Cells.Count} cells");
            }
            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            PrisonerMailInputModel[] dtoPrisoners = JsonConvert.DeserializeObject<PrisonerMailInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Prisoner> prisoners = new List<Prisoner>();

            foreach (PrisonerMailInputModel dtoPrisoner in dtoPrisoners)
            {
                if (!IsValid(dtoPrisoner) || !dtoPrisoner.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                bool IsValidIncarcerationDate = DateTime.TryParseExact(dtoPrisoner.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime incarcerationDate);
                if (!IsValidIncarcerationDate)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                DateTime? prisonerReleaseDate = DateTime.TryParseExact(dtoPrisoner.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate) ? (DateTime?)releaseDate : null;

                InitializeAutoMapper();

                Prisoner newPrisoner = mapper.Map<Prisoner>(dtoPrisoner);

                newPrisoner.ReleaseDate = prisonerReleaseDate;

                //Prisoner newPrisoner = new Prisoner
                //{
                //    FullName = dtoPrisoner.FullName,
                //    Nickname = dtoPrisoner.Nickname,
                //    Age = dtoPrisoner.Age,
                //    IncarcerationDate = incarcerationDate,
                //    ReleaseDate = prisonerReleaseDate,
                //    Bail = dtoPrisoner.Bail,
                //    CellId = dtoPrisoner.CellId,
                //    Mails = dtoPrisoner.Mails.Select(m=> new Mail
                //    {
                //        Description = m.Description,
                //        Sender = m.Sender,
                //        Address = m.Address
                //    })
                //    .ToArray()
                //};

                prisoners.Add(newPrisoner);
                sb.AppendLine($"Imported {newPrisoner.FullName} {newPrisoner.Age} years old");

            }
            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Officers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OfficerPrisonerInputModel[]), xmlRootAttribute);
            using StringReader reader = new StringReader(xmlString);
            OfficerPrisonerInputModel[] dtoOfficersPrisoners = (OfficerPrisonerInputModel[])xmlSerializer.Deserialize(reader);

            StringBuilder sb = new StringBuilder();

            List<Officer> officers = new List<Officer>();   

            foreach (OfficerPrisonerInputModel dtoOfficerPrisoner in dtoOfficersPrisoners)
            {
                if (!IsValid(dtoOfficerPrisoner))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                //Officer newOfficer = new Officer
                //{
                //    FullName = dtoOfficerPrisoner.FullName,
                //    Salary = dtoOfficerPrisoner.Salary,
                //    Position = Enum.Parse<Position>(dtoOfficerPrisoner.Position),
                //    Weapon = Enum.Parse<Weapon>(dtoOfficerPrisoner.Weapon),
                //    DepartmentId = dtoOfficerPrisoner.DepartmentId,
                //    OfficerPrisoners = dtoOfficerPrisoner.Prisoners.Select(x => new OfficerPrisoner
                //    {
                //        PrisonerId = x.Id
                //    })
                //    .ToArray()

                //};
                InitializeAutoMapper();

                Officer newOfficer = mapper.Map<Officer>(dtoOfficerPrisoner);

                officers.Add(newOfficer);
               
                sb.AppendLine($"Imported {newOfficer.FullName} ({newOfficer.OfficerPrisoners.Count} prisoners)");
            }
            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }

        private static void InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            cfg.AddProfile<SoftJailProfile>());

            mapper = config.CreateMapper();
        }
    }
}