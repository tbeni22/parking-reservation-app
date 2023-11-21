namespace parking_reservation_app.Data
{
    public class ParkingPlace
    {
        public int ID { get; init; }
        public bool DisabledParking { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
