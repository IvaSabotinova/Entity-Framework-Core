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

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SupplierInputModel>), xmlRootAttribute);

            StringReader reader = new StringReader(inputXml);

            List<SupplierInputModel> dtoSuppliers = (List<SupplierInputModel>)xmlSerializer.Deserialize(reader);

            InitializeAutoMapper();

            IEnumerable<Supplier> suppliers = mapper.Map<IEnumerable<Supplier>>(dtoSuppliers);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";

        }

        //Query 10. Import Parts

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PartInputModel>), xmlRootAttribute);

            StringReader reader = new StringReader(inputXml);

            List<PartInputModel> dtoParts = xmlSerializer.Deserialize(reader) as List<PartInputModel>;

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
            XmlRootAttribute rootAttribute = new XmlRootAttribute("Cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CarInputModel>), rootAttribute);

            StringReader reader = new StringReader(inputXml);

            List<CarInputModel> dtoCars = xmlSerializer.Deserialize(reader) as List<CarInputModel>;

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
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Customers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CustomerInputModel>), xmlRootAttribute);
            StringReader reader = new StringReader(inputXml);
            List<CustomerInputModel> dtoCustomers = xmlSerializer
                .Deserialize(reader) as List<CustomerInputModel>;

            InitializeAutoMapper();

            List<Customer> customers = mapper.Map<List<Customer>>(dtoCustomers);

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }

        //Query 13. Import Sales

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Sales");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SaleInputModel>), xmlRootAttribute);
            StringReader stringReader = new StringReader(inputXml);
            List<SaleInputModel> dtoSales = xmlSerializer.Deserialize(stringReader) as List<SaleInputModel>;

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

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CarOutputModel>), xmlRootAttribute);

            StringWriter writer = new StringWriter();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            xmlSerializer.Serialize(writer, cars, namespaces);

            return writer.ToString();
        }

        //Query 15. Export Cars from make BMW

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            InitializeAutoMapper();

            List<CarMakeBMWOutputModel> dtoBMWCars = mapper.Map<List<CarMakeBMWOutputModel>>(context.Cars
                .Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance));

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CarMakeBMWOutputModel>), xmlRootAttribute);
            StringWriter writer = new StringWriter();

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            xmlSerializer.Serialize(writer, dtoBMWCars, namespaces);

            return writer.ToString();
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

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("suppliers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SupplierOutputModel>), xmlRootAttribute);

            StringWriter writer = new StringWriter();

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            xmlSerializer.Serialize(writer, dtoSuppliers, namespaces);

            return writer.ToString();

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

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CarOutputModelWithParts>), xmlRootAttribute);

            StringWriter writer = new StringWriter();

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");
            xmlSerializer.Serialize(writer, dtoCars, xmlSerializerNamespaces);

            return writer.ToString();
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
                     SpentMoney = x.Sales.Select(s=>s.Car).SelectMany(pc=>pc.PartCars).Sum(p=>p.Part.Price)

                 })
                 .OrderByDescending(x => x.SpentMoney)
                 .ToList();
           

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("customers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CustomerOutputModel>), xmlRootAttribute);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            StringWriter writer = new StringWriter();

            xmlSerializer.Serialize(writer, dtoCustomers, xmlSerializerNamespaces);

            return writer.ToString();

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
                PriceWithDiscount = x.Car.PartCars.Sum(p => p.Part.Price) - x.Car.PartCars.Sum(p => p.Part.Price) * x.Discount / 100

            })
            .ToList();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("sales");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SaleOutputModel>), xmlRootAttribute);
            StringWriter writer = new StringWriter();

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            xmlSerializer.Serialize(writer, sales, xmlSerializerNamespaces);

            return writer.ToString();
        }
        private static void InitializeAutoMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

            mapper = mapperConfiguration.CreateMapper();
        }

    }
}