using Microsoft.EntityFrameworkCore;

using PatientService.Models;

namespace PatientService.Data
{
    public class PatientDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientContact> PatientContacts { get; set; }

        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
