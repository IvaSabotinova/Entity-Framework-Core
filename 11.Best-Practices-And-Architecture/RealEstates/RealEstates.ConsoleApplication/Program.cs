using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using RealEstates.Services.Models_DTO_;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.ConsoleApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            ApplicationDbContext db = new ApplicationDbContext();
            db.Database.Migrate();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Property search");
                Console.WriteLine("2. Most expensive districts");
                Console.WriteLine("3. Average price per square meter");
                Console.WriteLine("0. EXIT");
                bool parsed = int.TryParse(Console.ReadLine(), out int option);
                if (parsed && option == 0)
                {
                    break;
                }
                if (parsed && option >= 1 && option <= 3)
                {
                    switch (option)
                    {
                        case 1: PropertySearch(db); break;
                        case 2: MostExpensiveDistricts(db); break;
                        case 3: AveragePricePerSquareMeter(db); break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static void AveragePricePerSquareMeter(ApplicationDbContext dbContext)
        {
            IPropertieService propertieService = new PropertiesService(dbContext);
            Console.WriteLine($"Average price per square meter: {propertieService.AveragePricePerSquareMeter():0.00}€/m²");
        }

        private static void MostExpensiveDistricts(ApplicationDbContext db)
        {
            Console.Write("Districts count: ");
            int count = int.Parse(Console.ReadLine());
            IDistrictsService districtService = new DistrictsService(db);
            IEnumerable<DistrictInfoDTO> districts = districtService.GetMostExpensiveDistricts(count);
            foreach (DistrictInfoDTO district in districts)
            {
                Console.WriteLine($"{district.Name} => {district.AveragePricePerSquareMeter:0.00}€/m² ({district.PropertiesCount})");
            }
        }

        private static void PropertySearch(ApplicationDbContext db)
        {
            Console.Write("Min price: ");
            int minPrice = int.Parse(Console.ReadLine());
            Console.Write("Max price: ");
            int maxPrice = int.Parse(Console.ReadLine());
            Console.Write("Min size: ");
            int minSize = int.Parse(Console.ReadLine());
            Console.Write("Max size: ");
            int maxSize = int.Parse(Console.ReadLine());

            IPropertieService service = new PropertiesService(db);

            IEnumerable<PropertyInfoDTO> properties = service.Search(minPrice, maxPrice, minSize, maxSize);

            foreach (PropertyInfoDTO property in properties)
            {
                Console.WriteLine($"{property.DistrictName}; {property.BuildingType}; {property.PropertyType} => {property.Price}€ => {property.Size}m²");
            }

        }
    }
}
