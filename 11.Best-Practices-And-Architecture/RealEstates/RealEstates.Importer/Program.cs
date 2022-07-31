using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RealEstates.Importer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ImportJsonFile("imot.bg-houses-Sofia-raw-data-2021-03-18.json");
            Console.WriteLine();
            ImportJsonFile("imot.bg-raw-data-2021-03-18.json");

        }
        public static void ImportJsonFile(string fileName)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            IPropertieService propertiesService = new PropertiesService(dbContext);
            IEnumerable<PropertyAsJson> properties = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>(File.ReadAllText(fileName));

            foreach (PropertyAsJson property in properties)
            {
                propertiesService.Add(property.District, property.Floor, property.TotalFloors, property.Size,
                    property.YardSize, property.Year, property.Type, property.BuildingType, property.Price);
                Console.Write(".");
            }
        }
    }
}
