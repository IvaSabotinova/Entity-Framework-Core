using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using System;

namespace P03_SalesDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SalesContext salesContext = new SalesContext();

            ResetDatabase(salesContext);

        }

        private static void ResetDatabase(SalesContext salesContext)
        {
            salesContext.Database.EnsureDeleted();
            salesContext.Database.Migrate();

            Seed(salesContext);
        }

        private static void Seed(SalesContext salesContext)
        {
            Product[] products = new Product[]
            {
                new Product
                {
                    Name = "Shampoo",
                    Description = "Suitable for long curly hair",
                    Price = 1.20m,
                    Quantity = 3
                },
                new Product
                {
                    Name = "Deodorant",
                    Description = "For men",
                    Price = 4.50m,
                    Quantity = 1
                },
                new Product
                {
                   Name = "Spirytus",
                   Description = "For disinfection",
                   Price = 1.10m,
                   Quantity = 1
                }
             };

            salesContext.Products.AddRange(products);

            Customer[] customers = new Customer[]
            {
                new Customer
                {
                     Name = "Pesho",
                     CreditCardNumber = "1111 1111 1111 1111",
                     Email = "pesho@abv.bg"
                },
                new Customer
                {
                     Name = "Gosho",
                     CreditCardNumber = "2222 2222 2222 2222",
                     Email = "gosho@abv.bg"
                },
                new Customer
                {
                     Name = "Maimun",
                     CreditCardNumber = "5555 5555 5555 5555",
                     Email = "maimun@abv.bg"
                }

             };

            salesContext.Customers.AddRange(customers);


            Store[] stores = new Store[]
            {
                new Store
                {
                     Name = "Picadilly"
                },
                new Store
                {
                     Name = "Kaufland"
                },
                new Store
                {
                     Name = "Metro"
                }
            };

            salesContext.Stores.AddRange(stores);


            Sale[] sales = new Sale[]
            {
                new Sale
                {
                      Date = new DateTime(2021,05,06),
                      Product = products[0],
                      Customer = customers[0],
                      Store = stores[0]
                },
                new Sale
                {
                      Date = new DateTime(2021,01,20),
                      Product = products[0],
                      Customer = customers[1],
                      Store = stores[2]
                },
                new Sale
                {
                      Date = new DateTime(2021,03,09),
                      Product = products[2],
                      Customer = customers[1],
                      Store = stores[0]
                }
            };

            salesContext.Sales.AddRange(sales);

            salesContext.SaveChanges();
        }
    }
}
