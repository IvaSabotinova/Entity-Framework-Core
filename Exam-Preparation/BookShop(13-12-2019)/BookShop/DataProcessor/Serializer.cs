namespace BookShop.DataProcessor
{
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
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
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {

            var authors = context.Authors.Select(x => new
               {
                   AuthorName = x.FirstName + " " + x.LastName,
                   Books = x.AuthorsBooks
                   .OrderByDescending(b => b.Book.Price)
                   .Select(b => new
                   {
                       BookName = b.Book.Name,
                       BookPrice = b.Book.Price.ToString("f2")
                   })
                  .ToArray()
               })
               .ToList()
               .OrderByDescending(x => x.Books.Count())
               .ThenBy(x => x.AuthorName)
               .ToList();

            return JsonConvert.SerializeObject(authors, Formatting.Indented);

        }
          
        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            BookOutputModel[] dtoBooks = context.Books
                .Where(x => x.PublishedOn < date && x.Genre == Genre.Science)
                .OrderByDescending(x => x.Pages)       
                .ThenByDescending(x=>x.PublishedOn)
                .Take(10)
                .Select(x => new BookOutputModel
                {
                    Name = x.Name,
                    Date = x.PublishedOn.ToString("d", CultureInfo.InvariantCulture),
                    Pages = x.Pages
                })
                .ToArray();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Books");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BookOutputModel[]), xmlRootAttribute);
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");
            StringBuilder sb = new StringBuilder();
            
           xmlSerializer.Serialize(new StringWriter(sb), dtoBooks, xmlSerializerNamespaces);

           return sb.ToString().TrimEnd();

        }
    }
}