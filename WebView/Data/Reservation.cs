namespace parking_reservation_app.Data
{
    public class Reservation
    {
        public int ID { get; init; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }

        public virtual ParkingPlace ParkingPlace { get; set; }
        public virtual User User { get; set; }
    }
}
