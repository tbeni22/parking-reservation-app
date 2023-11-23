namespace parking_reservation_app.DAL
{
    using Microsoft.EntityFrameworkCore;
    using parking_reservation_app.Data;


    namespace ContosoUniversity.DAL
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


        }
    }
}
