
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            ShellOutputModel[] dtoShells = context.Shells.Where(x => x.ShellWeight > shellWeight)
            .Select(x => new ShellOutputModel
            {
                ShellWeight = x.ShellWeight,
                Caliber = x.Caliber,
                Guns = x.Guns.Where(g => g.GunType == GunType.AntiAircraftGun)
                .Select(g => new GunJsonOutputModel
                {
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight,
                    BarrelLength = g.BarrelLength,
                    Range = g.Range > 3000 ? "Long-range" : "Regular range",


                })
                .OrderByDescending(x => x.GunWeight)
                .ToArray(),
            })
            .OrderBy(x => x.ShellWeight)
            .ToArray();

            return JsonConvert.SerializeObject(dtoShells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            GunOutputModel[] dtoGuns = context.Guns.Where(x => x.Manufacturer.ManufacturerName == manufacturer).Select(x => new GunOutputModel
            {
                ManufacturerName = x.Manufacturer.ManufacturerName,
                GunType = x.GunType.ToString(),
                GunWeight = x.GunWeight,
                BarrelLength = x.BarrelLength,
                Range = x.Range,
                Countries = x.CountriesGuns.Where(c => c.Country.ArmySize > 4500000)
                  .Select(c => new CountryOutputModel
                  {
                      CountryName = c.Country.CountryName,
                      ArmySize = c.Country.ArmySize
                  })
                  .OrderBy(x=>x.ArmySize)
                  .ToArray()


            })
            .OrderBy(x=>x.BarrelLength)
            .ToArray();

            XmlRootAttribute xmlAttribute = new XmlRootAttribute("Guns");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GunOutputModel[]), xmlAttribute);
            
            StringBuilder sb = new StringBuilder();

            using StringWriter writer = new StringWriter(sb);
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            xmlSerializer.Serialize(writer, dtoGuns, xmlSerializerNamespaces);

            return writer.ToString();
        }
    }
}
