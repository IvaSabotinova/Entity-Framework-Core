using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTO.InputDTOs;
using ProductShop.DTO.OutputDTOs;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static IMapper mapper;
        public static void Main(string[] args)
        {
            ProductShopContext productShopContext = new ProductShopContext();
            //productShopContext.Database.EnsureDeleted();
            //productShopContext.Database.EnsureCreated();

            //string inputXmlUsers = File.ReadAllText("./Datasets/users.xml");
            //string resultProblem01 = ImportUsers(productShopContext, inputXmlUsers);
            //Console.WriteLine(resultProblem01);  // Query 1. Import Users

            //string inputXmlProducts = File.ReadAllText("./Datasets/products.xml");
            //string resultproblem02 = ImportProducts(productShopContext, inputXmlProducts);
            //Console.WriteLine(resultproblem02); //Query 2. Import Products

            //string inputXmlCategories = File.ReadAllText("./Datasets/categories.xml");
            //string resultProblem03 = ImportCategories(productShopContext, inputXmlCategories);
            //Console.WriteLine(resultProblem03);  //Query 3. Import Categories

            //string inputXmlCategoriesProducts = File.ReadAllText("./Datasets/categories-products.xml");
            //string resultProblem04 = ImportCategoryProducts(productShopContext, inputXmlCategoriesProducts);
            //Console.WriteLine(resultProblem04); //Query 4. Import Categories and Products

            //Console.WriteLine(GetProductsInRange(productShopContext)); //Query 5. Export Products In Range

            //Console.WriteLine(GetSoldProducts(productShopContext)); //Query 6. Export Sold Products

            //Console.WriteLine(GetCategoriesByProductsCount(productShopContext)); //Query 7. Export Categories By Products Count

            Console.WriteLine(GetUsersWithProducts(productShopContext));  //Query 8. Export Users and Products

        }
        //1.Import Data

        // Query 1. Import Users

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            List<UserInputModel> dtoUsers = Deserialize<UserInputModel>(inputXml, "Users");

            InitializeAutoMapper();

            List<User> users = mapper.Map<List<User>>(dtoUsers);

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";

        }

        //Query 2. Import Products

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            List<ProductInputModel> dtoProducts = Deserialize<ProductInputModel>(inputXml, "Products");

            InitializeAutoMapper();
            List<Product> products = mapper.Map<List<Product>>(dtoProducts);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}"; ;
        }

        //Query 3. Import Categories

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            List<CategoryInputModel> dtoCategories = Deserialize<CategoryInputModel>(inputXml, "Categories");

            InitializeAutoMapper();
            List<Category> categories = mapper.Map<List<Category>>(dtoCategories.Where(x => x.Name != null));

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //Query 4. Import Categories and Products

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            List<int> validCategoriesIds = context.Categories.Select(x => x.Id).ToList();
            List<int> validProductsIds = context.Products.Select(x => x.Id).ToList();

            List<CategoryProductInputModel> allDtoCategoriesProducts = Deserialize<CategoryProductInputModel>(inputXml, "CategoryProducts");

            List<CategoryProductInputModel> dtoCategoriesProducts = allDtoCategoriesProducts
                .Where(x => validCategoriesIds.Contains(x.CategoryId))
                .Where(x => validProductsIds.Contains(x.ProductId))
                .ToList();

            InitializeAutoMapper();
            List<CategoryProduct> categoriesProducts = mapper.Map<List<CategoryProduct>>(dtoCategoriesProducts);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";

        }

        //2. Query and Export Data

        //Query 5. Export Products In Range

        public static string GetProductsInRange(ProductShopContext context)
        {
            List<ProductInRangeOutputModel> dtoPoducts = context.Products
               .Where(x => x.Price >= 500 && x.Price <= 1000)
               .Select(x => new ProductInRangeOutputModel
               {
                   Name = x.Name,
                   Price = x.Price,
                   BuyerName = x.Buyer.FirstName + " " + x.Buyer.LastName

               })
               .OrderBy(x => x.Price)
               .Take(10)
               .ToList();

            string result = Serialize<ProductInRangeOutputModel>(dtoPoducts, "Products");
            return result;
        }

        //Query 6. Export Sold Products

        public static string GetSoldProducts(ProductShopContext context)
        {
            List<UserAndProductsOutputModel> users = context.Users
                .Where(x => x.ProductsSold.Count > 0)
                .Select(x => new UserAndProductsOutputModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(p => new SoldProductOutputModel
                    {
                        Name = p.Name,
                        Price = p.Price

                    }).ToList()

                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToList();

            string result = Serialize<UserAndProductsOutputModel>(users, "Users");
            return result;
        }

        //Query 7. Export Categories By Products Count

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            List<CategoryOutputModel> dtoCategories = context.Categories
                .Select(x => new CategoryOutputModel
                {
                    Name = x.Name,
                    NumberOfProducts = x.CategoryProducts.Count,
                    AverageProductsPrice = x.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)

                })
                .OrderByDescending(x => x.NumberOfProducts)
                .ThenBy(x => x.TotalRevenue)
                .ToList();

            string result = Serialize<CategoryOutputModel>(dtoCategories, "Categories");
            return result;

        }

        //Query 8. Export Users and Products

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            List<UserAndProductCountOutputModel> users = context.Users
                .Include(x => x.ProductsSold) // not needed, added for Judge & ToList()                
                .Where(x => x.ProductsSold.Count > 0)
                .OrderByDescending(x => x.ProductsSold.Count)
                .ToList() // not needed, added for Judge
                .Select(u => new UserAndProductCountOutputModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductCountOutputModel
                    {
                        ProductsCount = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new ProductOutputModel
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(x => x.Price)
                        .ToList()
                    }
                })
                .Take(10)
                .ToList();


            FinalResult newObject = new FinalResult
            {
                CountOfUsers = context.Users.Count(x => x.ProductsSold.Any()),
                Users = users
            };

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Users");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FinalResult), xmlRootAttribute);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder(); 
            using (StringWriter writer = new StringWriter(sb))
            {
               xmlSerializer.Serialize(writer, newObject, namespaces);

            }
            return sb.ToString().TrimEnd();

        }

        private static void InitializeAutoMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

            mapper = mapperConfiguration.CreateMapper();
        }
        private static List<T> Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>), xmlRootAttribute);

            List<T> dtoT = new List<T>();
            using (StringReader reader = new StringReader(inputXml))
            {
                dtoT = (List<T>)xmlSerializer.Deserialize(reader);
            }
            return dtoT;
        }
        private static string Serialize<T>(List<T> dtoT, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>), xmlRootAttribute);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, dtoT, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}