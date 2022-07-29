using Newtonsoft.Json;
using System.Collections.Generic;

namespace VaporStore.DataProcessor.Dto.Export
{
    [JsonObject]
    public class GenreWithGamesOutputModel
    {
        public int Id { get; set; }

        [JsonProperty("Genre")]
        public string Name { get; set; }

        [JsonProperty("Games")]
        public GameOutputModel[] Games { get; set; }

        public int TotalPlayers {get; set; }    

    }
    public class GameOutputModel
    {
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Name { get; set; }

        [JsonProperty("Developer")]
        public string DeveloperName { get; set; }
     
        [JsonProperty("Tags")]
        public string TagsNames { get; set; }

        [JsonProperty("Players")]
        public int PurchaseCount { get; set; }   
    }
}

