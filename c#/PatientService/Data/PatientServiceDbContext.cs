using Microsoft.EntityFrameworkCore;

using PatientService.Models;

namespace PatientService.Data
{
    public class PatientServiceDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientContact> PatientContacts { get; set; }

        public PatientServiceDbContext(DbContextOptions<PatientServiceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasNoKey();
            modelBuilder.Entity<PatientContact>().OwnsOne(e => e.Address).WithOwner(e => e.PatientContact);
        }
    }
}
