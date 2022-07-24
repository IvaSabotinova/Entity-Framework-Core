using Newtonsoft.Json;

namespace ProductShop.DataTransferObjects_DTOs_.OutputModels
{
    [JsonObject]
    public class ProductInfoOutputModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("buyerFirstName")]
        public string BuyerFirstName { get; set; }

        [JsonProperty("buyerLastName")]
        public string BuyerLastName { get; set; }
    }
}