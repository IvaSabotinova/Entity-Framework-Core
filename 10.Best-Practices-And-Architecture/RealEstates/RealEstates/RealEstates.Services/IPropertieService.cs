using RealEstates.Services.Models_DTO_;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface IPropertieService
    {
        void Add(string district, int floor, int maxFloor, int size, int yardSize,
            int year, string propertyType, string buildingType, int price);

        decimal AveragePricePerSquareMeter();

        IEnumerable<PropertyInfoDTO> Search(int minPrice, int maxPrice, int minSize, int maxSize);

    }
}
