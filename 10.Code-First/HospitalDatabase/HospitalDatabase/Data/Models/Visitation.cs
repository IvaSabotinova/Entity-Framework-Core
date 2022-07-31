using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Visitation
    {
        public int VisitationId { get; set; }   
        public DateTime Date { get; set; }

        [StringLength(Constants.VisitationCommentsMaxLength)]
        public string Comments { get; set; }

        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }  
        public Patient Patient { get; set; }

        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

    }

}
