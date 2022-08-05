namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
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
        static IMapper mapper;
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {

            //var dtoPrisoners = context.Prisoners.Where(x => ids.Contains(x.Id))
            //.Select(x => new
            //{
            //    Id = x.Id,
            //    Name = x.FullName,
            //    CellNumber = x.Cell.CellNumber,
            //    Officers = x.PrisonerOfficers.Select(po => new
            //    {
            //        OfficerName = po.Officer.FullName,
            //        Department = po.Officer.Department.Name
            //    })
            //    .OrderBy(x => x.OfficerName)
            //    .ToArray(),
            //    TotalOfficerSalary = decimal.Parse(x.PrisonerOfficers.Sum(po => po.Officer.Salary).ToString("f2")),

            //})
            //.OrderBy(x => x.Name)
            //.ThenBy(x => x.Id)
            //.ToArray();

            PrisonerWithOfficersOutputModel[] dtoPrisoners = context.Prisoners
                .Where(x=>ids.Contains(x.Id))
                .ProjectTo<PrisonerWithOfficersOutputModel>(InitializeAutoMapperConfig())
                .OrderBy(x => x.FullName) 
                .ThenBy(x=>x.Id)
                .ToArray(); 

            return JsonConvert.SerializeObject(dtoPrisoners, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] namesOfPrisoners = prisonersNames.Split(",");
            //PrisonerOutputModel[] dtoPrisoners = context.Prisoners.Where(x => namesOfPrisoners.Contains(x.FullName))
            //    .Select(x => new PrisonerOutputModel
            //    {
            //        Id = x.Id,
            //        FullName = x.FullName,
            //        IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            //        Mails = x.Mails.Select(m => new MailOfPrisonerOutputModel
            //        {
            //            Description = string.Join("", m.Description.Reverse())

            //        })
            //        .ToArray(),

            //    })
            //    .OrderBy(x => x.FullName)
            //    .ThenBy(x => x.Id)
            //    .ToArray();
            PrisonerOutputModel[] dtoPrisoners = context.Prisoners
                .Where(x => namesOfPrisoners.Contains(x.FullName))
                .ProjectTo<PrisonerOutputModel>(InitializeAutoMapperConfig())
                .OrderBy(x => x.FullName)
                .ThenBy(x => x.Id)
                .ToArray();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Prisoners");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PrisonerOutputModel[]), xmlRootAttribute);

            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, dtoPrisoners, xmlSerializerNamespaces);
            return writer.ToString().TrimEnd();
        }
        private static MapperConfiguration InitializeAutoMapperConfig()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            cfg.AddProfile<SoftJailProfile>());

            mapper = config.CreateMapper();
            return config;

        }
    }
}