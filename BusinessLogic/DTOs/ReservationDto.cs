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

        //from Reservation to ReservationDto, name of the func is FromReservation 
        public static ReservationDto FromReservation(Reservation reservation)
        {
            return new ReservationDto()
            {
                ID = reservation.ID,
                Beginning = reservation.Beginning,
                Ending = reservation.Ending,
                ParkingPlaceId = reservation.ParkingPlaceId,
                ParkingPlaceName = reservation.ParkingPlace.Name,
                User = UserDto.FromUser(reservation.User)
            };
        }

        //to Reservation from ReservationDto, name of the func is ToReservation
        public static Reservation ToReservation(ReservationDto reservationDto)
        {
            return new Reservation()
            {
                ID = reservationDto.ID,
                Beginning = reservationDto.Beginning,
                Ending = reservationDto.Ending,
                ParkingPlaceId = reservationDto.ParkingPlaceId,
                User = UserDto.ToUser(reservationDto.User)
            };
        }
       
    }

    
}
