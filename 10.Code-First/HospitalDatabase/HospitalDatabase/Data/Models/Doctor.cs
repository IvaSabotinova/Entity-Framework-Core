using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Doctor
    {
        public Doctor()
        {
            Visitations = new HashSet<Visitation>();
        }
        public int DoctorId { get; set; }

        [MaxLength(Constants.DoctorNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(Constants.DoctorSpecialtyMaxLength)]
        public string Specialty { get; set; } 
        
        public ICollection<Visitation> Visitations { get; set; }

    }
}