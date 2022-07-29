namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            PrisonerByCells[] prisoners = context.Prisoners.Where(x => ids.Contains(x.Id))
                .Select(x => new PrisonerByCells
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers.Select(po => new OfficerOutputModel
                    {
                        FullName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(x => x.FullName)
                    .ToArray(),
                    TotalOfficerSalary = decimal.Parse(x.PrisonerOfficers.Sum(o => o.Officer.Salary).ToString("f2"))

                })
                .OrderBy(x => x.FullName)
                .ThenBy(x => x.Id)
                .ToArray();

            return JsonConvert.SerializeObject(prisoners, Formatting.Indented);

        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] prisonersNamesInput = prisonersNames.Split(",");
            PrisonerInbox[] prisoners = context.Prisoners
                .Where(x => prisonersNamesInput.Contains(x.FullName))
                .Select(x => new PrisonerInbox
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Mails = x.Mails.Select(m => new EncryptedMail
                    {
                        Description = string.Join("", m.Description.Reverse())
                    })
                .ToArray()
                })
                .OrderBy(x => x.FullName)
                .ThenBy(x => x.Id)
                .ToArray();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Prisoners");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PrisonerInbox[]), xmlRootAttribute);

            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces  xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, prisoners, xmlSerializerNamespaces);
            return writer.ToString().TrimEnd();
           
        }
    }
}