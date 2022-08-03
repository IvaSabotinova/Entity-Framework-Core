namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                .ToList() //added for Judge
                .Where(x => x.NumberOfHalls >= numbersOfHalls && x.Tickets.Count >= 20)
                .Select(x => new
                {
                    Name = x.Name,
                    Halls = x.NumberOfHalls,
                    TotalIncome = x.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Sum(t => t.Price),
                    Tickets = x.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                    .OrderByDescending(t => t.Price)
                    .Select(t => new
                    {
                        Price = decimal.Parse(t.Price.ToString("f2")),
                        RowNumber = t.RowNumber
                    })
                    .ToArray()
                })
                .OrderByDescending(x=>x.Halls)
                .ThenBy(x=>x.Name)
                .ToArray();


            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            PlayOutputModel[] playes = context.Plays.ToList().Where(x=>x.Rating <= rating)
               .Select(x=> new PlayOutputModel
               {
                  Title = x.Title,
                  Duration = x.Duration.ToString("c"),
                  Rating = x.Rating == 0 ? "Premier" : x.Rating.ToString(), 
                  Genre = x.Genre.ToString(),
                  Actors = x.Casts.Where(a=>a.IsMainCharacter)
                  .Select(a=> new Actor
                  {
                      FullName = a.FullName,                       
                      MainCharacter = $"Plays main character in '{x.Title}'."
                  })
                  .OrderByDescending(x=>x.FullName)
                  .ToArray(),
               })
               .OrderBy(x=>x.Title)
               .ThenByDescending(x=>x.Genre)
               .ToArray();


            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Plays");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayOutputModel[]), xmlRootAttribute);
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            xmlSerializer.Serialize(writer, playes, xmlSerializerNamespaces);

            return writer.ToString();
        }
    }
}
