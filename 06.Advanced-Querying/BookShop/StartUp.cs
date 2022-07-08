namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            string command = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, command));  //T02. Age Restriction

            //Console.WriteLine(GetGoldenBooks(db));  //T03. Golden Books

            //Console.WriteLine(GetBooksByPrice(db));   //T04.Books by Price

            //Console.WriteLine(GetBooksNotReleasedIn(db, int.Parse(command))); //T05.Not Released In

            //Console.WriteLine(GetBooksByCategory(db, command)); // T06. Book Titles by Category

            //Console.WriteLine(GetBooksReleasedBefore(db, command)); //T07. Released Before Date

            //Console.WriteLine(GetAuthorNamesEndingIn(db, command)); //T08. Author Search

            //Console.WriteLine(GetBookTitlesContaining(db, command));   //T09. Book Search

            //Console.WriteLine(GetBooksByAuthor(db, command)); //T10. Book Search by Author

            //Console.WriteLine(CountBooks(db, int.Parse(command))); //T11. Count Books

            //Console.WriteLine(CountCopiesByAuthor(db)); //T12. Total Book Copies

            //Console.WriteLine(GetTotalProfitByCategory(db));  //T13. Profit by Category

            //Console.WriteLine(GetMostRecentBooks(db)); //T14. Most Recent Books

            //IncreasePrices(db); //T15. Increase Prices

            Console.WriteLine(RemoveBooks(db));  //T16.Remove Books

        }

        //T02. Age Restriction

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            //AgeRestriction ageRestrictionInput = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            AgeRestriction ageRestrictionInput = Enum.Parse<AgeRestriction>(command, true);

            List<string> booksTitles = context.Books
                .Where(x => x.AgeRestriction == ageRestrictionInput)
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            StringBuilder sb = new StringBuilder();

            return string.Join(Environment.NewLine, booksTitles);
        }

        //T03. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            List<string> books = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);

        }

        //T04.Books by Price

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price).Select(x => new
                {
                    x.Title,
                    x.Price
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");

            }
            return sb.ToString().TrimEnd();
        }

        //T05.Not Released In

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            List<string> books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();
            return string.Join(Environment.NewLine, books);
        }

        // T06. Book Titles by Category

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            List<string> books = context.Books
                .Where(x => x.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, books);

        }

        //T07. Released Before Date

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateTimeInput = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate.Value < dateTimeInput)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price

                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType.ToString()} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //T08. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
            .Where(x => x.FirstName.EndsWith(input))
            .Select(x => new
            {
                FullName = x.FirstName + " " + x.LastName

            })
            .OrderBy(x => x.FullName)
            .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }
            return sb.ToString().TrimEnd();
        }

        //T09. Book Search

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            List<string> books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        //T10. Book Search by Author

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
             .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
             .OrderBy(x => x.BookId)
             .Select(x => new
             {
                 x.Title,
                 AuthorName = x.Author.FirstName + " " + x.Author.LastName
             })
             .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorName})");
            }
            return sb.ToString().TrimEnd();
        }

        //T11. Count Books

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Where(x => x.Title.Length > lengthCheck).Count();
        }

        //T12. Total Book Copies

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorsBookCopies = context.Authors.Select(x => new
            {
                AuthorFullName = x.FirstName + " " + x.LastName,
                TotalBookCopies = x.Books.Sum(x => x.Copies)

            })
            .OrderByDescending(x => x.TotalBookCopies)
            .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var author in authorsBookCopies)
            {
                sb.AppendLine($"{author.AuthorFullName} - {author.TotalBookCopies}");
            }
            return sb.ToString().TrimEnd();
        }

        //T13. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
            .Select(x => new
            {
                CategoryName = x.Name,
                ProfitPerCategory = x.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies)
            })
            .OrderByDescending(x => x.ProfitPerCategory)
            .ThenBy(x => x.CategoryName)
            .ToList();

            return string.Join(Environment.NewLine, categories.Select(x => $"{x.CategoryName} ${x.ProfitPerCategory:f2}"));

        }

        //T14. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var booksPerCategory = context.Categories
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                CategoryName = x.Name,
                CategoryBooks = x.CategoryBooks.Select(x => new
                {
                    BookTitle = x.Book.Title,
                    BookReleaseDate = x.Book.ReleaseDate
                })
                .OrderByDescending(x => x.BookReleaseDate)
                .Take(3)
            })
            .ToList();

            return string.Join(Environment.NewLine, booksPerCategory.Select(x => $"--{x.CategoryName}" + Environment.NewLine +
                   string.Join(Environment.NewLine, x.CategoryBooks.Select(x => $"{x.BookTitle} ({x.BookReleaseDate.Value.Year})"))));
        }

        //T15. Increase Prices

        public static void IncreasePrices(BookShopContext context)
        {
            List<Book> books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (Book book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();

            //Another solution

            //List<Book> books = context.Books
            // .Where(x => x.ReleaseDate.Value.Year < 2010)
            // .ToList();

            //foreach (Book book in books)
            //{
            //    book.Price += 5;
            //}
            //context.BulkUpdate(books);


            //One more solution not working in Judge, working locally

            //context.Books.Where(x => x.ReleaseDate.Value.Year < 2010)
            //     .Update(x => new Book { Price = x.Price + 5 });

        }

        //T16.Remove Books

        public static int RemoveBooks(BookShopContext context)
        {
           return context.Books.Where(x => x.Copies < 4200).Delete();
        }

    }
}
