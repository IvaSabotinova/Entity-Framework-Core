using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Diagnose
    {
        public int DiagnoseId { get; set; }

        [MaxLength(Constants.DiagnoseNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(Constants.DiagnoseCommentsMaxLength)]
        public string Comments { get; set; }

        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

    }

}
