using RealEstates.Services.Models_DTO_;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface IDistrictsService
    {
        IEnumerable<DistrictInfoDTO> GetMostExpensiveDistricts(int count);

    }
}
