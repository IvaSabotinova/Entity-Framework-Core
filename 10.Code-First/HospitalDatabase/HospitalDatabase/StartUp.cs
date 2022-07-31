using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;
using System;

namespace P01_HospitalDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            HospitalContext hospitalContext = new HospitalContext();

            ResetDatabase(hospitalContext);

        }

        private static void ResetDatabase(HospitalContext hospitalContext)
        {
            hospitalContext.Database.EnsureDeleted();
            hospitalContext.Database.Migrate();

            Seed(hospitalContext);

        }

        private static void Seed(HospitalContext hospitalContext)
        {
            Patient[] patients = new Patient[]
            {
                new Patient()
                {
                    FirstName = "Gosho1",
                    LastName = "Goshev1",
                    Address = "22, Park1, Burgas",
                    Email = "gosho1@abv.bg",
                    HasInsurance = true
                },
                new Patient()
                {
                    FirstName = "Gosho2",
                    LastName = "Goshev2",
                    Address = "22, Park2, Burgas",
                    Email = "gosho2@abv.bg",
                    HasInsurance = true
                },
                new Patient()
                {
                    FirstName = "Gosho3",
                    LastName = "Goshev3",
                    Address = "22, Park3, Burgas",
                    Email = "gosho3@abv.bg",
                    HasInsurance = true
                },

            };
            hospitalContext.Patients.AddRange(patients);
            
            Doctor[] doctors = new Doctor[]
            {
                new Doctor()
                {
                    Name = "Pesho",
                    Specialty = "Orthopaedist"
                },
                new Doctor()
                {
                    Name = "Gosho",
                    Specialty = "Dentist"
                },
                new Doctor()
                {
                    Name = "Geca",
                    Specialty = "Opthalmologist"
                }
             };

            hospitalContext.Doctors.AddRange(doctors);
           
            Visitation[] visitations = new Visitation[]
            {
                new Visitation()
                {
                    Date = new DateTime(2021, 01, 01),
                    Comments = "Blablabla1",
                    Doctor = doctors[0],
                    Patient = patients[0]
                },
                new Visitation()
                {
                    Date = new DateTime(2021, 01, 20),
                    Comments = "Blablabla2",
                    Doctor = doctors[1],
                    Patient = patients[1]
                },
                new Visitation()
                {
                    Date = new DateTime(2021, 01, 29),
                    Comments = "Blablabla3",
                    Doctor = doctors[2],
                    Patient = patients[2]
                },
             };
            hospitalContext.Visitations.AddRange(visitations);
            
            Diagnose[] diagnoses = new Diagnose[]
            {
                new Diagnose()
                {
                    Name = "Diagnose1",
                    Comments = "Bbbbb",
                    Patient = patients[0]
                },
                new Diagnose()
                {
                    Name = "Diagnose2",
                    Comments = "Ccccc",
                    Patient = patients[1]
                },
                new Diagnose()
                {
                    Name = "Diagnose3",
                    Comments = "Ddddd",
                    Patient = patients[2]
                }
            };

            hospitalContext.Diagnoses.AddRange(diagnoses);
           
            Medicament[] medicaments = new Medicament[]
            {
                new Medicament()
                {
                    Name = "medicament1"
                },
                new Medicament()
                {
                    Name = "medicament2"
                },
                new Medicament()
                {
                    Name = "medicament3"
                }
             };
            hospitalContext.Medicaments.AddRange(medicaments);
            
            PatientMedicament[] prescriptions = new PatientMedicament[]
            {
                new PatientMedicament()
                {
                     PatientId = 1,
                     Medicament = medicaments[0]
                },
                new PatientMedicament()
                {
                     PatientId = 2,
                     Medicament = medicaments[1]
                },
                new PatientMedicament()
                {
                     PatientId = 3,
                     Medicament = medicaments[2]
                }
            };
            hospitalContext.PatientMedicaments.AddRange(prescriptions);

            hospitalContext.SaveChanges();

        }
    }
}
