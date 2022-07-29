namespace SoftJail.DataProcessor
{

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
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {

            DepartmentCellInputModel[] departmentsCells = JsonConvert.DeserializeObject<DepartmentCellInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Department> departments = new List<Department>();
            foreach (DepartmentCellInputModel dept in departmentsCells)
            {
                if (!IsValid(dept) || !dept.Cells.All(IsValid) || !dept.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department newDepartment = new Department
                {
                    Name = dept.Name,
                    Cells = dept.Cells.Select(x => new Cell
                    {
                        CellNumber = x.CellNumber,
                        HasWindow = x.HasWindow,
                    })
                     .ToArray()
                };
                departments.Add(newDepartment);
                sb.AppendLine($"Imported {newDepartment.Name} with {newDepartment.Cells.Count} cells");
            }
            context.Departments.AddRange(departments);
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


                Prisoner newPrisoner = new Prisoner
                {
                    FullName = dtoPrisoner.FullName,
                    Nickname = dtoPrisoner.Nickname,    
                    Age = dtoPrisoner.Age,
                    IncarcerationDate = DateTime.ParseExact(dtoPrisoner.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = DateTime.TryParseExact(dtoPrisoner.ReleaseDate, "dd/MM/yyyy",
                                  CultureInfo.InvariantCulture,
                                  DateTimeStyles.None,
                                  out DateTime releaseDate) ? (DateTime?)releaseDate : null,
                    Bail = dtoPrisoner.Bail,
                    CellId = dtoPrisoner.CellId,
                    Mails = dtoPrisoner.Mails.Select(m => new Mail
                    {
                        Description = m.Description,
                        Sender = m.Sender,
                        Address = m.Address

                    }).ToArray()
                };
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
            OfficerPrisonerInputModel[] officersPrisoners = (OfficerPrisonerInputModel[])xmlSerializer.Deserialize(reader);

            List<Officer> officers = new List<Officer>();
            StringBuilder sb = new StringBuilder();

            foreach (OfficerPrisonerInputModel officerPrisoners in officersPrisoners)
            {
                if (!IsValid(officerPrisoners) || !officerPrisoners.Prisoners.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                Officer newofficer = new Officer
                {

                    FullName = officerPrisoners.FullName,
                    Salary = officerPrisoners.Salary,
                    Position = Enum.Parse<Position>(officerPrisoners.Position),
                    Weapon = Enum.Parse<Weapon>(officerPrisoners.Weapon),
                    DepartmentId = officerPrisoners.DepartmentId,
                    OfficerPrisoners = officerPrisoners.Prisoners.Select(x=> new OfficerPrisoner
                    {
                        PrisonerId = x.Id
                    })
                    .ToList()   
                };

                officers.Add(newofficer);
                sb.AppendLine($"Imported {newofficer.FullName} ({newofficer.OfficerPrisoners.Count} prisoners)");

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
    }
}