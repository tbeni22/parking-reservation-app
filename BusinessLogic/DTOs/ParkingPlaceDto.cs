using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ParkingPlaceDto
    {
        public int ID { get; init; }
        public bool DisabledParking { get; set; }

        public ICollection<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
    }
}
