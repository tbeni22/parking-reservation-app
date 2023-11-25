using Microsoft.EntityFrameworkCore;
using DataAccess.Data;

namespace DataAccess
{
    public class ParkingContext : DbContext
    {

        public ParkingContext() :base() { }

        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ParkingPlace> ParkingPlaces { get; set; }
        public DbSet<FailureReport> FailureReports { get; set; }


    }
}
