using Newtonsoft.Json;

namespace ProductShop.DataTransferObjects_DTOs_.OutputModels
{
    [JsonObject]
    public class ProductOfUserOutputModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

    }
}
