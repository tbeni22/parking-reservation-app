using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ReservationDto
    {
        public int ID { get; init; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }

        public ParkingPlaceDto? ParkingPlace { get; set; }


        public UserDto? User { get; set; }

        public static ReservationDto FromDataEntity(Reservation r)
        {
            return new ReservationDto
            {
                ID = r.ID,
                Beginning = r.Beginning,
                Ending = r.Ending,
                ParkingPlace = ParkingPlaceDto.FromDataEntity(r.ParkingPlace),
                User = UserDto.FromDataEntity(r.User)
            };
        }
    }
}
