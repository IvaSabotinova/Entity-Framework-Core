using HospitalDatabase;
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext :DbContext
    {
        public HospitalContext()
        {

        }
        public HospitalContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {

        }
        public DbSet<Diagnose> Diagnoses { get; set; }  

        public DbSet<Doctor> Doctors { get; set; }  
        public DbSet<Medicament> Medicaments { get; set; }  
        public DbSet<Patient> Patients { get; set; }  
        public DbSet<PatientMedicament> PatientMedicaments { get; set; }
        public DbSet<Visitation> Visitations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>().
                HasKey(x => new { x.PatientId, x.MedicamentId });


            base.OnModelCreating(modelBuilder);
        }





    }
}
