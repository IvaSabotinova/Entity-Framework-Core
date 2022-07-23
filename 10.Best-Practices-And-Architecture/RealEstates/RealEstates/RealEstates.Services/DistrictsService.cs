using RealEstates.Data;
using RealEstates.Services.Models_DTO_;
using System.Collections.Generic;
using System.Linq;

namespace RealEstates.Services
{
    public class DistrictsService : IDistrictsService
    {
        private readonly ApplicationDbContext dbContext;

        public DistrictsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<DistrictInfoDTO> GetMostExpensiveDistricts(int count)
        {
            List<DistrictInfoDTO> districts = dbContext.Districts.Select(x => new DistrictInfoDTO
            {
                Name = x.Name,
                PropertiesCount = x.Properties.Count(),
                AveragePricePerSquareMeter = x.Properties
                .Where(x => x.Price.HasValue)
                .Average(x => x.Price / (decimal)x.Size) ?? 0
            })
                .OrderByDescending(x => x.AveragePricePerSquareMeter)
                .Take(count)
                .ToList();

            return districts;
        }
    }
}
