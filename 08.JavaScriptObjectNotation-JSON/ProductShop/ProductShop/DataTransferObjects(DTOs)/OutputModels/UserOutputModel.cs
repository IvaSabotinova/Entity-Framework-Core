using Newtonsoft.Json;

namespace ProductShop.DataTransferObjects_DTOs_.OutputModels
{
    [JsonObject]
    public class UserOutputModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ProductInfoOutputModel[] SoldProducts { get; set; }


    }
}
