using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models_DTO_;
using System.Collections.Generic;
using System.Linq;

namespace RealEstates.Services
{
    public class PropertiesService : IPropertieService
    {
        private readonly ApplicationDbContext dbContext;

        public PropertiesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(string district, int floor, int maxFloor, int size, int yardSize,
            int year, string propertyType, string buildingType, int price)
        {
            Property property = new Property()
            {
                Size = size,
                Price = price == 0 ? null : price,
                Floor = floor == 0 || floor > 255 ? null : (byte)floor,
                TotalFloors = maxFloor == 0 || maxFloor > 255 ? null : (byte)maxFloor,
                YardSize = yardSize == 0 ? null : yardSize,
                Year = year <= 1800 ? null : year,
            };

            District dbDistrict = dbContext.Districts.FirstOrDefault(x => x.Name == district);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }
            property.District = dbDistrict;

            PropertyType dbPropertyType = dbContext.PropertyTypes.FirstOrDefault(x => x.Name == propertyType);

            if (dbPropertyType == null)
            {
                dbPropertyType = new PropertyType { Name = propertyType };
            }
            property.Type = dbPropertyType;

            BuildingType dbBuildingType = dbContext.BuildingTypes.FirstOrDefault(x => x.Name == buildingType);

            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }
            property.BuildingType = dbBuildingType;

            dbContext.Properties.Add(property);
            dbContext.SaveChanges();
        }

        public decimal AveragePricePerSquareMeter()
        {
            return dbContext.Properties.Where(x=>x.Price.HasValue)
                .Average(x=>x.Price / (decimal)x.Size) ?? 0;
        }

        public IEnumerable<PropertyInfoDTO> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            List<PropertyInfoDTO> properties = dbContext.Properties
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.Size >= minSize && x.Size <= maxSize)
                .Select(x => new PropertyInfoDTO
                {
                    Size = x.Size,
                    Price = x.Price ?? 0,
                    BuildingType = x.BuildingType.Name,
                    DistrictName = x.District.Name,
                    PropertyType = x.Type.Name

                })
                .ToList();

            return properties;
        }
    }
}
