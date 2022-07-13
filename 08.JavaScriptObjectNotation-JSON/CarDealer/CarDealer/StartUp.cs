using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            //1. Setup Database

            CarDealerContext carDealerContext = new CarDealerContext();
            carDealerContext.Database.EnsureDeleted();
            carDealerContext.Database.EnsureCreated();

            string inputJsonSuppliers = File.ReadAllText("../../../Datasets/suppliers.json");
            string resultProblem09 = ImportSuppliers(carDealerContext, inputJsonSuppliers);
            //Console.WriteLine(resultProblem09); //Query 9. Import Suppliers

            string inputJsonParts = File.ReadAllText("../../../Datasets/parts.json");
            string resultProblem10 = ImportParts(carDealerContext, inputJsonParts);
            //Console.WriteLine(resultProblem10); //Query 10. Import Parts

            string inputJsonCars = File.ReadAllText("../../../Datasets/cars.json");
            string resultProblem11 = ImportCars(carDealerContext, inputJsonCars);
            //Console.WriteLine(resultProblem11); //Query 11. Import Cars

            string inputJsonCustomers = File.ReadAllText("../../../Datasets/customers.json");
            string resultProblem12 = ImportCustomers(carDealerContext, inputJsonCustomers);
            //Console.WriteLine(resultProblem12);   //Query 12. Import Customers

            string inputJsonSales = File.ReadAllText("../../../Datasets/sales.json");
            string resultproblem13 = ImportSales(carDealerContext, inputJsonSales);
            //Console.WriteLine(resultproblem13); //Query 13. Import Sales

            //Console.WriteLine(GetOrderedCustomers(carDealerContext));  //Query 14. Export Ordered Customers

            //Console.WriteLine(GetCarsFromMakeToyota(carDealerContext)); //Query 15. Export Cars from Make Toyota

            //Console.WriteLine(GetLocalSuppliers(carDealerContext)); //16. Export Local Suppliers

            //Console.WriteLine(GetCarsWithTheirListOfParts(carDealerContext));  //Query 17. Export Cars with Their List of Parts

            //Console.WriteLine(GetTotalSalesByCustomer(carDealerContext)); //Query 18. Export Total Sales by Customer

            //Console.WriteLine(GetSalesWithAppliedDiscount(carDealerContext)); //Query 19. Export Sales with Applied Discount

        }

        //2.Import Data

        //Query 9. Import Suppliers

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();

            IEnumerable<SupplierInputModel> dtoSuppliers = JsonConvert.DeserializeObject<IEnumerable<SupplierInputModel>>(inputJson);

            IEnumerable<Supplier> suppliers = mapper.Map<IEnumerable<Supplier>>(dtoSuppliers);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";

        }

        //Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();

            List<int> suppliersIds = context.Suppliers.Select(x => x.Id).ToList();

            IEnumerable<PartInputModel> dtoParts = JsonConvert.DeserializeObject<IEnumerable<PartInputModel>>(inputJson).Where(x => suppliersIds.Contains(x.SupplierId));

            IEnumerable<Part> parts = mapper.Map<IEnumerable<Part>>(dtoParts);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";

        }

        //Query 11. Import Cars

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IEnumerable<CarInputModel> dtoCars = JsonConvert.DeserializeObject<IEnumerable<CarInputModel>>(inputJson);

            List<Car> cars = new List<Car>();

            foreach (CarInputModel dtoCar in dtoCars)
            {
                Car newCar = new Car
                {
                    Make = dtoCar.Make,
                    Model = dtoCar.Model,
                    TravelledDistance = dtoCar.TravelledDistance,
                };
                foreach (int partId in dtoCar.PartsId.Distinct())
                {
                    newCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                cars.Add(newCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";
        }

        //Query 12. Import Customers

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IEnumerable<CustomerInputModel> dtoCustomers = JsonConvert.DeserializeObject<IEnumerable<CustomerInputModel>>(inputJson);

            InitializeAutoMapper();

            IEnumerable<Customer> customers = mapper.Map<IEnumerable<Customer>>(dtoCustomers);

            context.Customers.AddRange(customers);
            context.SaveChanges();


            return $"Successfully imported {customers.Count()}.";
        }

        //Query 13. Import Sales

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IEnumerable<SaleInputModel> dtoSales = JsonConvert.DeserializeObject<IEnumerable<SaleInputModel>>(inputJson);

            InitializeAutoMapper();

            IEnumerable<Sale> sales = mapper.Map<IEnumerable<Sale>>(dtoSales);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }

        private static void InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            mapper = config.CreateMapper();
        }

        //3. Export Data

        //Query 14. Export Ordered Customers

        public static string GetOrderedCustomers(CarDealerContext context)
        {

            var customers = context.Customers
            .OrderBy(x => x.BirthDate)
            .ThenBy(x => x.IsYoungDriver == true)
            .Select(x => new
            {
                x.Name,
                BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                x.IsYoungDriver
            })
            .ToList();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        //Query 15. Export Cars from Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                }).ToList();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);

        }

        //16. Export Local Suppliers

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToList();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        //Query 17. Export Cars with Their List of Parts

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars.Select(x => new
            {
                car = new
                {
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                },
                parts = x.PartCars.Select(y => new
                {
                    y.Part.Name,
                    Price = y.Part.Price.ToString("F2")
                })
            })
           .ToList();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        //Query 18. Export Total Sales by Customer

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
            .Where(x => x.Sales.Count > 0)
            .Select(x => new
            {
                fullName = x.Name,
                boughtCars = x.Sales.Count,
                spentMoney = x.Sales.Sum(y => y.Car.PartCars.Sum(p => p.Part.Price))
            })
           .OrderByDescending(x => x.spentMoney)
           .ThenByDescending(x => x.boughtCars)
           .ToList();

            return JsonConvert.SerializeObject(customers, Formatting.Indented); 
        }

        //Query 19. Export Sales with Applied Discount

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Select(x => new
            {
                car = new
                {
                    x.Car.Make,
                    x.Car.Model,
                    x.Car.TravelledDistance
                },
                customerName = x.Customer.Name,
                Discount = x.Discount.ToString("F2"),
                price = x.Car.PartCars.Sum(p => p.Part.Price).ToString("F2"),
                priceWithDiscount = (x.Car.PartCars.Sum(p => p.Part.Price) - x.Car.PartCars.Sum(p => p.Part.Price) * x.Discount / 100).ToString("F2")
            })
            .Take(10)
            .ToList();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);

        }
    }
}