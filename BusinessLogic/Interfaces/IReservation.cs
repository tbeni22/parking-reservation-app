using BusinessLogic.DTOs;
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

        Task<int> NewReservation(ReservationDto data);

        Task<int> NewRepeatingReservation(RepeatingReservation data);

        Task DeleteReservation(int reservationId);

        Task<bool> NotifyAboutNoFreeSpace(ReservationDto slot);
    }
}
