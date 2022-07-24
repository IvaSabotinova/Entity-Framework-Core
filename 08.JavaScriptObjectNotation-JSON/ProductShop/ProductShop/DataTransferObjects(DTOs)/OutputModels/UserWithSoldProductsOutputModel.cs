using Newtonsoft.Json;

namespace ProductShop.DataTransferObjects_DTOs_.OutputModels
{

    [JsonObject]
    public class UserWithSoldProductsOutputModel
    {
        [JsonProperty("firstName")]
       public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("soldProducts")]
        public SoldProductOutputModel ProductSold { get; set; }


    }
}
