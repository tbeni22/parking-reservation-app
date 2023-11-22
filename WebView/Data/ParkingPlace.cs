using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace parking_reservation_app.Data
{
    [Table("ParkingPlace")]
    public class ParkingPlace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; init; }
        public bool DisabledParking { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
