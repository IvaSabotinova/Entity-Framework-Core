using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ExportDto
{
    [JsonObject]
    public class PrisonerByCells
    {
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string FullName { get; set; }

        [JsonProperty("CellNumber")]
        public int? CellNumber { get; set; }

        [JsonProperty("Officers")]
        public OfficerOutputModel[] Officers { get; set; }

        [JsonProperty("TotalOfficerSalary")]
       public decimal TotalOfficerSalary { get; set; } 

    }

    [JsonObject]
    public class OfficerOutputModel
    {
        [JsonProperty("OfficerName")]
        public string FullName { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

    }

}
