using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            Prescriptions = new HashSet<PatientMedicament>();
            Visitations = new HashSet<Visitation>();
            Diagnoses = new HashSet<Diagnose>();
        }
        public int PatientId { get; set; }

        [MaxLength(Constants.PatientFirstNameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(Constants.PatientLastNameMaxLength)]
        public string LastName { get; set; }

        [MaxLength(Constants.PatientAddressMaxLength)]
        public string Address { get; set; }

        [MaxLength(Constants.PatientEmailMaxLength)]
        public string Email { get; set; }
        public bool HasInsurance { get; set; }
        public ICollection<PatientMedicament> Prescriptions { get; set; }
        public ICollection<Visitation> Visitations { get; set; }    
        public ICollection<Diagnose> Diagnoses { get; set; }    
    }
}
