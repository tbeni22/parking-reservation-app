using BusinessLogic.DTOs;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IReservation
    {
        Task<List<ReservationDto>> GetReservationsForUser(int userId);

        Task<ReservationDto> GetReservation(int id);

        Task<ReservationDto> NewReservation(ReservationDto data);

        Task<bool> NewRepeatingReservation(RepeatingReservation data);

        Task<ReservationDto> DeleteReservation(int reservationId);

        Task<FailureReport> NotifyAboutNoFreeSpace(ReservationDto slot);

        Task<List<ParkingPlaceDto>> GetFreeSpaces(DateTime Start, DateTime End);
    }
}
