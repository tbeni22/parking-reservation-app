namespace parking_reservation_app.Data
{
    public class User
    {
        public int ID { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
