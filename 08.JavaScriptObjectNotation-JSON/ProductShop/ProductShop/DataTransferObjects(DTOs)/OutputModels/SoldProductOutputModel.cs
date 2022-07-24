using Newtonsoft.Json;

namespace ProductShop.DataTransferObjects_DTOs_.OutputModels
{
    [JsonObject]
    public class SoldProductOutputModel
    {
        [JsonProperty("count")]
        public int Count => this.ProductsOfUser.Length;

        [JsonProperty("products")]
        public ProductOfUserOutputModel[] ProductsOfUser { get; set; }  


    }
}
