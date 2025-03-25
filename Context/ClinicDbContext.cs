using Microsoft.AspNetCore.Identity;
using ClinicAppointmentSystemApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystemApp.Context
{
    public class ClinicDbContext : IdentityDbContext<User>
    {
        // this is used for connnection from database to create the identity
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


    }
}
