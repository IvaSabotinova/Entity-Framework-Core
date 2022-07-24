using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DataTransferObjects_DTOs_;
using ProductShop.DataTransferObjects_DTOs_.OutputModels;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            ProductShopContext productShopContext = new ProductShopContext();
            productShopContext.Database.EnsureDeleted();
            productShopContext.Database.EnsureCreated();

            string inputJsonUsers = File.ReadAllText("../../../Datasets/users.json");
            string resultProblem01 = ImportUsers(productShopContext, inputJsonUsers);
            //Console.WriteLine(resultProblem01); //Query 1. Import Users

            string inputJsonProducts = File.ReadAllText("../../../Datasets/products.json");
            string resultProblem02 = ImportProducts(productShopContext, inputJsonProducts);
            //Console.WriteLine(resultProblem02);  //Query 2. Import Products

            string inputJsonCategories = File.ReadAllText("../../../Datasets/categories.json");
            string resultProblem03 = ImportCategories(productShopContext, inputJsonCategories);
            //Console.WriteLine(resultProblem03);   //Query 3. Import Categories

            string inputJsonCategoriesProducts = File.ReadAllText("../../../Datasets/categories-products.json");
            string resultProblem04 = ImportCategoryProducts(productShopContext, inputJsonCategoriesProducts);
            //Console.WriteLine(resultProblem04);  //Query 4. Import Categories and Products

            //Console.WriteLine(GetProductsInRange(productShopContext)); //Query 5. Export Products in Range

            //Console.WriteLine(GetSoldProducts(productShopContext)); //Query 6. Export Sold Products

            //Console.WriteLine(GetCategoriesByProductsCount(productShopContext)); //Query 7. Export Categories by Products Count

            Console.WriteLine(GetUsersWithProducts(productShopContext)); //Query 8. Export Users and Products

        }

        //1. Import Data

        //Query 1. Import Users

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            IEnumerable<UserInputModel> dtoUsers = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);

            IEnumerable<User> users = mapper.Map<IEnumerable<User>>(dtoUsers);
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        //Query 2. Import Products

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            IEnumerable<ProductInputModel> dtoProducts = JsonConvert.DeserializeObject<IEnumerable<ProductInputModel>>(inputJson);

            IEnumerable<Product> products = mapper.Map<IEnumerable<Product>>(dtoProducts);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        //Query 3. Import Categories

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            IEnumerable<CategoryInputModel> dtoCategories = JsonConvert.DeserializeObject<IEnumerable<CategoryInputModel>>(inputJson)
                .Where(c => c.Name != null);

            IEnumerable<Category> categories = mapper.Map<IEnumerable<Category>>(dtoCategories);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        //Query 4. Import Categories and Products

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            IEnumerable<CategoryProductInputModel> dtoCategoryProduct = JsonConvert.DeserializeObject<IEnumerable<CategoryProductInputModel>>(inputJson);

            IEnumerable<CategoryProduct> categoryProduct = mapper.Map<IEnumerable<CategoryProduct>>(dtoCategoryProduct);

            context.CategoryProducts.AddRange(categoryProduct);
            context.SaveChanges();

            return $"Successfully imported {categoryProduct.Count()}";
        }
        private static void InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            cfg.AddProfile<ProductShopProfile>());

            mapper = config.CreateMapper();
        }

        //2. Export Data

        //Query 5. Export Products in Range

        public static string GetProductsInRange(ProductShopContext context)
        {
            //Solution without AutoMapper

            //var products = context.Products
            //    .Where(x => x.Price >= 500 && x.Price <= 1000)
            //    .OrderBy(x => x.Price)
            //    .Select(x => new
            //    {
            //        name = x.Name,
            //        price = x.Price,
            //        seller = x.Seller.FirstName + " " + x.Seller.LastName

            //    }).ToList();

            //return JsonConvert.SerializeObject(products, Formatting.Indented);

            //Another solution with AutoMapper

            ProductInRangeOutputModel[] products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .ProjectTo<ProductInRangeOutputModel>(InitializeAutoMapperConfig())
                .OrderBy(x => x.Price)
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);

        }

        //Query 6. Export Sold Products

        public static string GetSoldProducts(ProductShopContext context)
        {
            //Solution without AutoMapper

            //var users = context.Users
            //    .Where(x => x.ProductsSold.Any(y => y.BuyerId != null))
            //    .OrderBy(x => x.LastName)
            //    .ThenBy(x => x.FirstName)
            //    .Select(x => new
            //    {
            //        firstName = x.FirstName,
            //        lastName = x.LastName,
            //        soldProducts = x.ProductsSold
            //        .Where(p => p.BuyerId != null)
            //        .Select(y => new
            //        {
            //            name = y.Name,
            //            price = y.Price,
            //            buyerFirstName = y.Buyer.FirstName,
            //            buyerLastName = y.Buyer.LastName
            //        })
            //    })
            //    .ToList();

            //return JsonConvert.SerializeObject(users, Formatting.Indented);


            //Another solution with AutoMapper

            UserOutputModel[] users = context.Users
             .Where(x => x.ProductsSold.Any(p => p.BuyerId.HasValue))
             .OrderBy(x => x.LastName)
             .ThenBy(x => x.FirstName)
             .ProjectTo<UserOutputModel>(InitializeAutoMapperConfig())
             .ToArray();

            return JsonConvert.SerializeObject(users, Formatting.Indented);

        }

        //Query 7. Export Categories by Products Count

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            //Soultion without AutoMapper

            //var categories = context.Categories.Select(x => new
            //{
            //    category = x.Name,
            //    productsCount = x.CategoryProducts.Count,
            //    //averagePrice = $"{x.CategoryProducts.Average(y => y.Product.Price):f2}",
            //    //totalRevenue = $"{x.CategoryProducts.Sum(y => y.Product.Price):f2}"
            //    averagePrice = x.CategoryProducts.Count == 0 ? 0.ToString("F2")
            //    : x.CategoryProducts.Average(y => y.Product.Price).ToString("F2"),
            //    totalRevenue = x.CategoryProducts.Sum(y => y.Product.Price).ToString("f2")
            //})
            //.OrderByDescending(x => x.productsCount)
            //.ToList();

            //return JsonConvert.SerializeObject(categories, Formatting.Indented);


            //Another solution with AutoMapper

            CategoryByProductsCountOutputModel[] categories = context.Categories
                .ProjectTo<CategoryByProductsCountOutputModel>(InitializeAutoMapperConfig())
                .OrderByDescending(x => x.ProductsCount)
                .ToArray();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);

        }

        //Query 8. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //Solution without AutoMapper

            //var users = context.Users
            //    .Include(x => x.ProductsSold)
            //    .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
            //    .ToList()
            //    .Select(u => new
            //    {
            //        firstName = u.FirstName,
            //        lastName = u.LastName,
            //        age = u.Age,
            //        soldProducts = new
            //        {
            //            count = u.ProductsSold.Where(p => p.BuyerId != null).Count(),
            //            products = u.ProductsSold.Where(p => p.BuyerId != null)
            //            .Select(p => new
            //            {
            //                name = p.Name,
            //                price = p.Price
            //            })
            //        }
            //    })
            //    .OrderByDescending(u => u.soldProducts.count)
            //    .ToList();

            //var newObjectOfUsers = new
            //{
            //    usersCount = users.Count,
            //    users = users

            //};

            //JsonSerializerSettings settings = new JsonSerializerSettings()
            //{
            //    Formatting = Formatting.Indented,
            //    NullValueHandling = NullValueHandling.Ignore,
            //};

            //return JsonConvert.SerializeObject(newObjectOfUsers, settings);


            //Another solution with AutoMapper

            List<UserWithSoldProductsOutputModel> users = context.Users
                .Where(x => x.ProductsSold.Any(b => b.BuyerId.HasValue))
                .OrderByDescending(x => x.ProductsSold.Count(b => b.BuyerId.HasValue))
                .ProjectTo<UserWithSoldProductsOutputModel>(InitializeAutoMapperConfig())
                .ToList();

            UserCountOutputModel output = mapper.Map<UserCountOutputModel>(users);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            return JsonConvert.SerializeObject(output, settings);
        }

        private static MapperConfiguration InitializeAutoMapperConfig()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            cfg.AddProfile<ProductShopProfile>());

            mapper = config.CreateMapper();
            return config;

        }
    }
}