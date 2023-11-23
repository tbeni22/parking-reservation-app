using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace parking_reservation_app.Data
{
    [Table("Reservation")]
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; init; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }

        public int ParkingPlaceId { get; set; }

        [ForeignKey("ParkingPlaceId")]
        public ParkingPlace ParkingPlace { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
