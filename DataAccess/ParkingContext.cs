using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;

namespace DataAccess
{
    public class ParkingContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ParkingPlace> ParkingPlaces { get; set; }
        public DbSet<FailureReport> FailureReports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
