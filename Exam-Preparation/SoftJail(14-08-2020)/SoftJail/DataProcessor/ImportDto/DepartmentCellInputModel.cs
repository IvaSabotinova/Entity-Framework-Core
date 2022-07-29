using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    [JsonObject]
    public class DepartmentCellInputModel
    {
        [Required]
        [JsonProperty("Name")]

        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        [JsonProperty("Cells")]
        public CellInputModel[] Cells { get; set; }
    }

    public class CellInputModel
    {
        [Range(1, 1000)]
        public int CellNumber { get; set; }
        public bool HasWindow { get; set; }

    }

}
