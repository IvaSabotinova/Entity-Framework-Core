namespace BookShop.DataProcessor
{
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Books");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BookInputModel[]), xmlRootAttribute);
            BookInputModel[] dtoBooks;

            using (StringReader reader = new StringReader(xmlString))
            {
               dtoBooks = (BookInputModel[])xmlSerializer.Deserialize(reader);
            }
            StringBuilder sb = new StringBuilder();
            List<Book> books = new List<Book>();
            foreach (BookInputModel dtoBook in dtoBooks)
            {
                if (!IsValid(dtoBook))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                bool IsValidDate = DateTime.TryParseExact(dtoBook.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                if (!IsValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }
                Book newBook = new Book()
                {
                    Name = dtoBook.Name,
                    Genre = Enum.Parse<Genre>(dtoBook.Genre),
                    Pages = dtoBook.Pages,
                    Price = dtoBook.Price,
                    PublishedOn = date

                };
                books.Add(newBook);
                sb.AppendLine(String.Format(SuccessfullyImportedBook,newBook.Name, newBook.Price));
             }
            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            AuthorInputModel[] dtoAuthors = JsonConvert.DeserializeObject<AuthorInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder(); 

            foreach (AuthorInputModel dtoAuthor in dtoAuthors)
            {
                if(!IsValid(dtoAuthor))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Author authorEmail = context.Authors.FirstOrDefault(x => x.Email == dtoAuthor.Email);
                if(authorEmail != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Author newAuthor = new Author()
                {
                    FirstName = dtoAuthor.FirstName,
                    LastName = dtoAuthor.LastName,
                    Email = dtoAuthor.Email,
                    Phone = dtoAuthor.Phone
                };
                foreach (AuthorBooksInputModel dtoBook in dtoAuthor.Books)
                {
                    Book book = context.Books.FirstOrDefault(x => x.Id == dtoBook.Id);
                    if (book == null)
                    {
                        continue;
                    }

                    newAuthor.AuthorsBooks.Add(new AuthorBook
                    {
                        Author = newAuthor,
                        Book = book
                    });
                }
                
                if(newAuthor.AuthorsBooks.Count() == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
              
                context.Authors.Add(newAuthor);
                context.SaveChanges();
                sb.AppendLine(String.Format(SuccessfullyImportedAuthor, newAuthor.FirstName + " " + newAuthor.LastName, newAuthor.AuthorsBooks.Count));
            }
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}