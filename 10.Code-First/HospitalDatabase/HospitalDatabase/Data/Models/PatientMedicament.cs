using P01_HospitalDatabase.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class PatientMedicament
    {
        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }  
        public Patient Patient { get; set; }

        [ForeignKey(nameof(Medicament))]
        public int MedicamentId { get; set; }
        public Medicament Medicament { get; set; }

    }
}
