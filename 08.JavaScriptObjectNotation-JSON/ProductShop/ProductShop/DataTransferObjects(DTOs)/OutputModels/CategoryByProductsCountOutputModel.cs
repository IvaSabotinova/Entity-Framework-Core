using Newtonsoft.Json;

namespace ProductShop.DataTransferObjects_DTOs_.OutputModels
{

    [JsonObject]
    public class CategoryByProductsCountOutputModel
    {
        [JsonProperty("category")]
        public string CategoryName { get; set; }

        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public string ProductsAveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public string TotalRevenue { get; set; }


    }
}
