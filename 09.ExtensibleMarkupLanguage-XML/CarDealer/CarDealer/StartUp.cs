using AutoMapper;
using CarDealer.Data;
using CarDealer.DataTransferObjects.InputDTOs;
using CarDealer.DataTransferObjects.OutputDTOs;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static IMapper mapper;
        public static void Main(string[] args)
        {
            //1. Setup Database

            CarDealerContext carDealerContext = new CarDealerContext();

            //carDealerContext.Database.EnsureDeleted();
            //carDealerContext.Database.EnsureCreated();

            //string inputXmlSuppliers = File.ReadAllText("./Datasets/suppliers.xml");
            //string resultProblem09 = ImportSuppliers(carDealerContext, inputXmlSuppliers);
            //Console.WriteLine(resultProblem09); //Query 9. Import Suppliers

            //string inputXmlParts = File.ReadAllText("./Datasets/parts.xml");
            //string resultProblem10 = ImportParts(carDealerContext, inputXmlParts);
            //Console.WriteLine(resultProblem10);  //Query 10. Import Parts

            //string inputXmlCars = File.ReadAllText("./Datasets/cars.xml");
            //string resultProblem11 = ImportCars(carDealerContext, inputXmlCars);
            //Console.WriteLine(resultProblem11); //Query 11. Import Cars

            //string inputXmlCustomers = File.ReadAllText("./Datasets/customers.xml");
            //string resultProblem12 = ImportCustomers(carDealerContext, inputXmlCustomers);
            //Console.WriteLine(resultProblem12); //Query 12. Import Customers

            //string inputXmlSales = File.ReadAllText("./Datasets/sales.xml");
            //string resultProblem13 = ImportSales(carDealerContext, inputXmlSales);
            //Console.WriteLine(resultProblem13); //Query 13. Import Sales

            //Console.WriteLine(GetCarsWithDistance(carDealerContext)); //Query 14. Export Cars With Distance

            //Console.WriteLine(GetCarsFromMakeBmw(carDealerContext)); //Query 15. Export Cars from make BMW

            //Console.WriteLine(GetLocalSuppliers(carDealerContext)); //Query 16. Export Local Suppliers

            //Console.WriteLine(GetCarsWithTheirListOfParts(carDealerContext)); //Query 17. Export Cars with Their List of Parts

            //Console.WriteLine(GetTotalSalesByCustomer(carDealerContext)); //Query 18. Export Total Sales by Customer

            Console.WriteLine(GetSalesWithAppliedDiscount(carDealerContext)); //Query 19. Export Sales with Applied Discount

        }

        //2. Import Data

        //Query 9. Import Suppliers

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            List<SupplierInputModel> dtoSuppliers = Deserialize<SupplierInputModel>(inputXml, "Suppliers");

            InitializeAutoMapper();

            IEnumerable<Supplier> suppliers = mapper.Map<IEnumerable<Supplier>>(dtoSuppliers);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";

        }

        //Query 10. Import Parts

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            List<PartInputModel> dtoParts = Deserialize<PartInputModel>(inputXml, "Parts");

            InitializeAutoMapper();

            List<int> existingSuppliersIds = context.Suppliers.Select(x => x.Id).ToList();

            IEnumerable<Part> parts = mapper.Map<IEnumerable<Part>>(dtoParts).Where(x => existingSuppliersIds.Contains(x.SupplierId));

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";

        }

        //Query 11. Import Cars

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            List<CarInputModel> dtoCars = Deserialize<CarInputModel>(inputXml, "Cars");

            List<int> existingPartIds = context.Parts.Select(x => x.Id).ToList();

            List<Car> cars = dtoCars.Select(x => new Car
            {
                Make = x.Make,
                Model = x.Model,
                TravelledDistance = x.TravelledDistance,
                PartCars = x.PartsIds.Select(x => x.Id).Intersect(existingPartIds).Distinct()
                .Select(y => new PartCar
                {
                    PartId = y
                })
                 .ToList(),
            })
            .ToList();

            //List<Car> cars = new List<Car>();

            //foreach (CarInputModel dtoCar in dtoCars)
            //{
            //    Car newCar = new Car
            //    {
            //        Make = dtoCar.Make,
            //        Model = dtoCar.Model,
            //        TravelledDistance = dtoCar.TravelledDistance
            //    };

            //    List<int> carPartsCommon = dtoCar.PartsIds.Select(x => x.Id)
            //         .Intersect(existingPartIds).Distinct().ToList();

            //    foreach (int carPartId in carPartsCommon)
            //    {
            //        newCar.PartCars.Add(new PartCar
            //        {
            //            PartId = carPartId

            //        });

            //    }
            //    cars.Add(newCar);
            //};

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Query 12. Import Customers

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            List<CustomerInputModel> dtoCustomers = Deserialize<CustomerInputModel>(inputXml, "Customers");

            InitializeAutoMapper();

            List<Customer> customers = mapper.Map<List<Customer>>(dtoCustomers);

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }

        //Query 13. Import Sales

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            List<SaleInputModel> dtoSales = Deserialize<SaleInputModel>(inputXml, "Sales");

            List<int> existingCarsIds = context.Cars.Select(x => x.Id).ToList();

            InitializeAutoMapper();

            List<Sale> sales = mapper.Map<List<Sale>>(dtoSales).Where(x => existingCarsIds.Contains(x.CarId)).ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";

        }

        //3. Query and Export Data

        //Query 14. Export Cars With Distance

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            InitializeAutoMapper();

            List<CarOutputModel> dtoCars = mapper.Map<List<CarOutputModel>>(context.Cars);

            List<CarOutputModel> cars = dtoCars.Where(x => x.TravelledDistance > 2000000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToList();

            string result = Serialize<CarOutputModel>(cars, "cars");
            return result;
        }

        //Query 15. Export Cars from make BMW

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            InitializeAutoMapper();

            List<CarMakeBMWOutputModel> dtoBMWCars = mapper.Map<List<CarMakeBMWOutputModel>>(context.Cars
                .Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance));

            string result = Serialize<CarMakeBMWOutputModel>(dtoBMWCars, "cars");
            return result;

        }

        //Query 16. Export Local Suppliers

        public static string GetLocalSuppliers(CarDealerContext context)
        {

            List<Supplier> filteredSuppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Include(x => x.Parts)
                .ToList();

            InitializeAutoMapper();

            List<SupplierOutputModel> dtoSuppliers = mapper.Map<List<SupplierOutputModel>>(filteredSuppliers);

            string result = Serialize<SupplierOutputModel>(dtoSuppliers, "suppliers");
            return result;

        }

        //Query 17. Export Cars with Their List of Parts

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            List<CarOutputModelWithParts> dtoCars = context.Cars.Select(x => new CarOutputModelWithParts
            {
                Make = x.Make,
                Model = x.Model,
                TravelledDistance = x.TravelledDistance,
                CarParts = x.PartCars.Select(p => new CarPartsOutputModel
                {
                    Name = p.Part.Name,
                    Price = p.Part.Price

                })
                .OrderByDescending(x => x.Price)
                .ToList()

            })
            .OrderByDescending(x => x.TravelledDistance)
            .ThenBy(x => x.Model)
            .Take(5)
            .ToList();

            string result = Serialize<CarOutputModelWithParts>(dtoCars, "cars");
            return result;
        }

        //Query 18. Export Total Sales by Customer

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            List<CustomerOutputModel> dtoCustomers = context.Customers
                 .Where(x => x.Sales.Count > 0)
                 .Select(x => new CustomerOutputModel
                 {
                     Name = x.Name,
                     BoughtCars = x.Sales.Count,
                     SpentMoney = x.Sales.Select(s => s.Car).SelectMany(pc => pc.PartCars).Sum(p => p.Part.Price)

                 })
                 .OrderByDescending(x => x.SpentMoney)
                 .ToList();

            string result = Serialize<CustomerOutputModel>(dtoCustomers, "customers");
            return result;

        }

        //Query 19. Export Sales with Applied Discount

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            List<SaleOutputModel> sales = context.Sales.Select(x => new SaleOutputModel
            {
                SoldCar = new SoldCarOutputModel
                {
                    Make = x.Car.Make,
                    Model = x.Car.Model,
                    TravelledDistance = x.Car.TravelledDistance

                },
                Discount = x.Discount,
                CustomerName = x.Customer.Name,
                Price = x.Car.PartCars.Sum(p => p.Part.Price),
                PriceWithDiscount = x.Car.PartCars.Sum(p => p.Part.Price) - x.Car.PartCars.Sum(p => p.Part.Price) * (x.Discount / 100)

            })
            .ToList();

            string result = Serialize<SaleOutputModel>(sales, "sales");
            return result;
        }
        private static void InitializeAutoMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

            mapper = mapperConfiguration.CreateMapper();
        }

        private static List<T> Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>), xmlRootAttribute);

            using StringReader reader = new StringReader(inputXml);

            List<T> dtoT = (List<T>)xmlSerializer.Deserialize(reader);

            return dtoT;
        }

        private static string Serialize<T>(List<T> dtoT, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>), xmlRootAttribute);
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            xmlSerializer.Serialize(writer, dtoT, namespaces);

            return sb.ToString().TrimEnd();

        }

    }
}