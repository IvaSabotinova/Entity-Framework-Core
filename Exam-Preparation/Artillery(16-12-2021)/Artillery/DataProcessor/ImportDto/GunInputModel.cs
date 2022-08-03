using Artillery.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Artillery.DataProcessor.ImportDto
{

    [JsonObject]
    public class GunInputModel
    {
        [JsonProperty("ManufacturerId")]
        public int ManufacturerId { get; set; }

        [Range(100, 1_350_000)]
        [JsonProperty("GunWeight")]
        public int GunWeight { get; set; }

        [Range(2.00, 35.00)]
        [JsonProperty("BarrelLength")]
        public double BarrelLength { get; set; }

        [JsonProperty("NumberBuild")]
        public int? NumberBuild { get; set; }

        [Range(1, 100_000)]
        [JsonProperty("Range")]
        public int Range { get; set; }

        [Required]
        [EnumDataType(typeof(GunType))]
        [JsonProperty("GunType")]
        public string GunType { get; set; }

        [JsonProperty("ShellId")]
        public int ShellId { get; set; }

        [JsonProperty("Countries")]
        public CountriesOfGunInputModel[] Countries { get; set; }
    }

    [JsonObject]
    public class CountriesOfGunInputModel
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}