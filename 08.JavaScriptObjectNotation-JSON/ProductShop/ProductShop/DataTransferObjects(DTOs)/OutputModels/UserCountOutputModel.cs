using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProductShop.DataTransferObjects_DTOs_.OutputModels
{
    [JsonObject]
    public class UserCountOutputModel
    {
        public UserCountOutputModel()
        {
            Users = new List<UserWithSoldProductsOutputModel>();
        }
        [JsonProperty("usersCount")]
        public int UsersCount => this.Users.Count;

        [JsonProperty("users")]
        public List<UserWithSoldProductsOutputModel> Users { get; set; }


    }
}
