using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Medicament
    {
        public Medicament()
        {
            Prescriptions = new HashSet<PatientMedicament>();
        }
        public int MedicamentId { get; set; }

        [MaxLength(Constants.MedicamentNameMaxLength)]
        public string	Name  { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }

    }
}
