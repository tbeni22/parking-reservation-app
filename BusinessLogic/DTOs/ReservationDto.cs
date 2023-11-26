using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ReservationDto
    {
        public int ID { get; set; }
        [Required]
        public DateTime Beginning { get; set; }
        [Required]
        public DateTime Ending { get; set; }
        public int ParkingPlaceId { get; set; }
        public String ParkingPlaceName { get; set; }
        public UserDto User { get; set; }
    }
}
