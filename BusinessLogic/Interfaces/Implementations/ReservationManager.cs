using BusinessLogic.DTOs;
using DataAccess.Data;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLogic.Interfaces.Implementations
{
    public class ReservationManager : IReservation
    {
        private ParkingContext context;
        private IUserManagement userService;

        public ReservationManager(ParkingContext context, IUserManagement userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<List<ReservationDto>> GetReservationsForUser(int userId)
        {
            var query = from reservation in context.Reservations
                        where reservation.UserId == userId
                        select reservation;

            var entities = await query.Include(e => e.ParkingPlace).ToListAsync();
            List<ReservationDto> result = new List<ReservationDto>();
            foreach (var entity in entities)
                result.Add(
                    new ReservationDto()
                    {
                        ID = entity.ID,
                        Beginning = entity.Beginning,
                        Ending = entity.Ending,
                        ParkingPlaceId = entity.ParkingPlaceId,
                        ParkingPlaceName = entity.ParkingPlace.Name
                    }
                );
            return result;
        }

        public async Task<ReservationDto> GetReservation(int id)
        {
            var query = from reservation in context.Reservations
                        where reservation.ID == id
                        select reservation;
            var entity = await query.FirstOrDefaultAsync();
            if (entity != null)
                return new ReservationDto()
                {
                    ID = entity.ID,
                    Beginning = entity.Beginning,
                    Ending = entity.Ending,
                    ParkingPlaceId = entity.ParkingPlaceId,
                    ParkingPlaceName = entity.ParkingPlace.Name,
                    User = UserDto.FromUser(entity.User)
                };
            else return null;
        }

        public async Task<ReservationDto> NewReservation(ReservationDto data)
        {
            var currentUser = await userService.GetCurrentUser();

            Reservation newreservation = new Reservation()
                {
                    ID = data.ID,
                    Beginning = data.Beginning,
                    Ending = data.Ending,
                    ParkingPlaceId = data.ParkingPlaceId,
                    UserId = currentUser.Id
            };

            var query = from reservation in context.Reservations
                        where reservation.ParkingPlace.ID == data.ParkingPlaceId
                        && data.Beginning.CompareTo(reservation.Beginning) >= 0 && data.Beginning.CompareTo(reservation.Ending) <= 0
                        && data.Ending.CompareTo(reservation.Beginning) >= 0 && data.Ending.CompareTo(reservation.Ending) <= 0
                        select reservation;

            var query2 = from parkingplace in context.ParkingPlaces
                        where parkingplace.DisabledParking == true
                        && parkingplace.ID == data.ParkingPlaceId
                        select parkingplace;


            var query3 = (from reservation in context.Reservations
                         where reservation.User.Id == currentUser.Id && reservation.Beginning.Day == DateTime.Now.Day
                          select (reservation.Ending - reservation.Beginning).Hours)
                         .Sum();

            var entities = await query.ToListAsync();
            var disabled = await query2.FirstOrDefaultAsync();

            /*mozgássérült hely van*/
            if (disabled != null)
            {
                if (entities.Count == 0 && currentUser.Disabled == true && ((data.Ending-data.Beginning).Hours + query3) <= 12)
                {
                    var created = await context.Reservations.AddAsync(newreservation);
                    context.SaveChanges();
                    var entity = created.Entity;
                    return ReservationDto.FromReservation(entity);
                }
            }
            else
            {
                if (entities.Count == 0 && ((data.Ending - data.Beginning).TotalHours + query3) <= 12)
                {
                    var created = await context.Reservations.AddAsync(newreservation);
                    context.SaveChanges();
                    var entity = created.Entity;
                    return ReservationDto.FromReservation(entity);
                }
            }

            return null;
        }

        public async Task<bool> NewRepeatingReservation(RepeatingReservation data)
        {
            List<ReservationDto> reservations = new List<ReservationDto>();
            switch (data.Rate)
            {
                case RepetitionRate.Daily:
                    for (int i = 0; i < data.RepeateFor; i++)
                    {
                        reservations.Add(
                            new ReservationDto()
                            {
                                Beginning = data.FirstOccurence.Beginning.AddDays(i),
                                Ending = data.FirstOccurence.Ending.AddDays(i),
                                ParkingPlaceId = data.FirstOccurence.ParkingPlaceId,
                                ParkingPlaceName = data.FirstOccurence.ParkingPlaceName,
                                User = data.FirstOccurence.User
                            }
                        );
                    }
                    break;
                case RepetitionRate.Weekly:
                    for (int i = 0; i < data.RepeateFor; i++)
                    {
                        reservations.Add(
                            new ReservationDto()
                            {
                                Beginning = data.FirstOccurence.Beginning.AddDays(i*7),
                                Ending = data.FirstOccurence.Ending.AddDays(i*7),
                                ParkingPlaceId = data.FirstOccurence.ParkingPlaceId,
                                ParkingPlaceName = data.FirstOccurence.ParkingPlaceName,
                                User = data.FirstOccurence.User
                            }
                        );
                    }
                    break;  
            }

            // check for collision
            bool isReserved = false;
            foreach (ReservationDto res in reservations)
            {
                if (await CheckIfReserved(res) == false)
                {
                    isReserved = true;
                }
            }

            if( !isReserved )
            {
                foreach (ReservationDto res in reservations)
                {
                    await NewReservation(res);
                }
                return true;
            }

            return false;

        }

        public async Task<ReservationDto> DeleteReservation(int reservationId)
        {
            var entity = await context.Reservations.FindAsync(reservationId);
            if (entity != null)
            {
                context.Reservations.Remove(entity);
                await context.SaveChangesAsync();
            }
            return new ReservationDto()
            {
                ID = entity.ID,
                Beginning = entity.Beginning,
                Ending = entity.Ending,
                ParkingPlaceId = entity.ParkingPlaceId,
                ParkingPlaceName = entity.ParkingPlace.Name
            };
        }

        public async Task<FailureReport> NotifyAboutNoFreeSpace(ReservationDto slot)
        {
            var currentUser = await userService.GetCurrentUser();

            FailureReport failureReport = new FailureReport()
            {
                ID = slot.ID,
                UserId = currentUser.Id,
                Beginning = slot.Beginning,
                Ending = slot.Ending
            };

            var created = await context.FailureReports.AddAsync(failureReport);
            context.SaveChanges();
            var entity = created.Entity;
            return new FailureReport()
            {
                ID = entity.ID,
                UserId = currentUser.Id,
                Beginning = entity.Beginning,
                Ending = entity.Ending
            };
        }

        public async Task<List<ParkingPlaceDto>> GetFreeSpaces(DateTime Start, DateTime End)
        {
            List<ParkingPlaceDto> freeSpacesList = new List<ParkingPlaceDto>();

            var reserved = await (from reservation in context.Reservations
                                  where Start.CompareTo(reservation.Beginning) >= 0 && Start.CompareTo(reservation.Ending) <= 0
                                        && End.CompareTo(reservation.Beginning) >= 0 && End.CompareTo(reservation.Ending) <= 0
                                  select reservation.ParkingPlaceId)
                                  .ToListAsync();

            var freeSpaces = await (from parkingPlace in context.ParkingPlaces
                                    where !reserved.Contains(parkingPlace.ID)
                                    select parkingPlace)
                                    .ToListAsync();

            foreach (var f in freeSpaces)
            {
                freeSpacesList.Add(
                    new ParkingPlaceDto()
                    {
                        ID = f.ID,
                        Name = f.Name,
                        DisabledParking = f.DisabledParking,
                        Reservations = new List<ReservationDto>()
                    }
                );
            }

            return freeSpacesList;
        }

        public async Task<bool> CheckIfReserved(ReservationDto data)
        {
            var currentUser = await userService.GetCurrentUser();

            var query = from reservation in context.Reservations
                        where reservation.ParkingPlace.ID == data.ParkingPlaceId
                        && data.Beginning.CompareTo(reservation.Beginning) >= 0 && data.Beginning.CompareTo(reservation.Ending) <= 0
                        && data.Ending.CompareTo(reservation.Beginning) >= 0 && data.Ending.CompareTo(reservation.Ending) <= 0
                        select reservation;

            var query2 = from parkingplace in context.ParkingPlaces
                         where parkingplace.DisabledParking == true
                         && parkingplace.ID == data.ParkingPlaceId
                         select parkingplace;

            var entities = await query.ToListAsync();
            var disabled = await query2.FirstOrDefaultAsync();
            if (disabled != null)
            {
                if (entities.Count == 0 && currentUser.Disabled == true)
                {
                   return true;
                }
            }
            else if (entities.Count == 0)
            {
                return true;
            }

            return false;
        }
    }
}
