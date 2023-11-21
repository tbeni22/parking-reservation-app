namespace parking_reservation_app.DAL
{
    using parking_reservation_app.Data;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    namespace ContosoUniversity.DAL
    {
        public class ParkingContext : DbContext
        {

            public ParkingContext() : base("ParkingContext")
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Reservation> Reservations { get; set; }
            public DbSet<ParkingPlace> ParkingPlaces { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }
        }
    }
}
