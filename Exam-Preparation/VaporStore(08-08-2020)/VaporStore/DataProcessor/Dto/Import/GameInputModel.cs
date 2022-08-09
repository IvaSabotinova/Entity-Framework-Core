using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class GameInputModel
    {
        [Required]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

        [Required]
        [JsonProperty("Developer")]
        public string DeveloperName { get; set; }

        [Required]
        public string Genre { get; set; }

        [JsonProperty("Tags")]
        public string[] TagsNames { get; set; }
    }
   
}
