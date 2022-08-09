using Newtonsoft.Json;
using System.Collections.Generic;

namespace VaporStore.DataProcessor.Dto.Export
{
    [JsonObject]
    public class GenreOutputModel
    {
        public int Id { get; set; }

        [JsonProperty("Genre")]
        public string Name { get; set; }

        [JsonProperty("Games")]
        public GameOutputModel[] Games { get; set; }

        [JsonProperty("TotalPlayers")]

        public int TotalPurchaseCount { get; set; }
    }

    [JsonObject]
    public class GameOutputModel
    {
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Name { get; set; }

        [JsonProperty("Developer")]
        public string DeveloperName { get; set; }

        [JsonProperty("Tags")]
       public string Tags { get; set; }

        [JsonProperty("Players")]
        public int PurchasesCount { get; set; }

    }
}

