using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ExportDto
{
    public class PrisonerWithOfficersOutputModel
    {
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string FullName { get; set; }

        [JsonProperty("CellNumber")]
        public int? CellNumber { get; set; }

        [JsonProperty("Officers")]
        public OfficerOfPrisonerOutputModel[] Officers { get; set; }

        [JsonProperty("TotalOfficerSalary")]
        public decimal TotalOfficerSalary { get; set; }
    }

    public class OfficerOfPrisonerOutputModel
    {
        [JsonProperty("OfficerName")]
        public string FullName { get; set; }

        [JsonProperty("Department")]
        public string DepartmentName { get; set; }

    }
}
