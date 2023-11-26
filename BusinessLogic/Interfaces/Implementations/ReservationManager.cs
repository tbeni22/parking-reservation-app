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

        public ReservationManager(ParkingContext context)
        {
            this.context = context;
        }

        public async Task<List<ReservationDto>> GetReservationsForUser(int userId)
        {
            var query = from reservation in context.Reservations
                        where reservation.UserId == userId
                        select reservation;

            var entities = await query.ToListAsync();
            List<ReservationDto> result = new List<ReservationDto>();
            foreach (var entity in entities)
                result.Add(
                    new ReservationDto()
                    {
                        ID = entity.ID,
                        Beginning = entity.Beginning,
                        Ending = entity.Ending,
                        ParkingPlaceId = entity.ParkingPlaceId,
                        ParkingPlaceName = entity.ParkingPlace.Name,
                        User = ConvertToUserDto(entity.User)
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
                    User = ConvertToUserDto(entity.User)
                };
            else return null;
        }

        public async Task<ReservationDto> NewReservation(ReservationDto data)
        {
            // todo: get user from session

            Reservation newreservation = new Reservation()
                {
                    ID = data.ID,
                    Beginning = data.Beginning,
                    Ending = data.Ending,
                    ParkingPlaceId = data.ParkingPlaceId,
                    UserId = data.User.Id
                };

            var query = from reservation in context.Reservations
                        where reservation.ParkingPlace.ID == data.ParkingPlaceId
                        && data.Beginning.CompareTo(reservation.Beginning) >= 0 && data.Beginning.CompareTo(reservation.Ending) <= 0
                        && data.Ending.CompareTo(reservation.Beginning) >= 0 && data.Ending.CompareTo(reservation.Ending) <= 0
                        select reservation;

            var query2 = from parkingplace in context.ParkingPlaces
                        where parkingplace.DisabledParking == true
                        && parkingplace.ID == parkingplace.ID
                        select parkingplace;


            var query3 = (from reservation in context.Reservations
                         where reservation.User.Id == data.User.Id
                         select (reservation.Ending - reservation.Beginning).TotalHours)
                         .Sum();

            var entities = await query.ToListAsync();
            var disabled = await query2.FirstOrDefaultAsync();
            /*mozgássérült hely van*/
            if (disabled != null)
            {
                if (entities.Count == 0 && data.User.Disabled == true && ((data.Ending-data.Beginning).TotalHours + query3) <= 12)
                {
                    var created = await context.Reservations.AddAsync(newreservation);
                    context.SaveChanges();
                    var entity = created.Entity;
                    return data;
                }
            }
            else
            {
                if (entities.Count == 0 && ((data.Ending - data.Beginning).TotalHours + query3) <= 12)
                {
                    var created = await context.Reservations.AddAsync(newreservation);
                    context.SaveChanges();
                    var entity = created.Entity;
                    return data;
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
                                Ending = data.FirstOccurence.Beginning.AddDays(i),
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
                                Ending = data.FirstOccurence.Beginning.AddDays(i*7),
                                ParkingPlaceId = data.FirstOccurence.ParkingPlaceId,
                                ParkingPlaceName = data.FirstOccurence.ParkingPlaceName,
                                User = data.FirstOccurence.User
                            }
                        );
                    }
                    break;  
            }

            bool isReserved = false;
            foreach (ReservationDto res in reservations)
            {
                if (await NewReservationRepeat(res) == false)
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
                ParkingPlaceName = entity.ParkingPlace.Name,
                User = ConvertToUserDto(entity.User)
            };
        }

        public async Task<FailureReport> NotifyAboutNoFreeSpace(ReservationDto slot)
        {
            FailureReport failureReport = new FailureReport()
            {
                ID = slot.ID,
                UserId = slot.User.Id,
                Beginning = slot.Beginning,
                Ending = slot.Ending
            };

            var created = await context.FailureReports.AddAsync(failureReport);
            context.SaveChanges();
            var entity = created.Entity;
            return new FailureReport()
            {
                ID = entity.ID,
                UserId = entity.User.Id,
                Beginning = entity.Beginning,
                Ending = entity.Ending
            };
        }

        public UserDto ConvertToUserDto(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Reservations = user.Reservations
            };
        }

        public async Task<bool> NewReservationRepeat(ReservationDto data)
        {
            // todo: get user from session

            Reservation newreservation = new Reservation()
            {
                ID = data.ID,
                Beginning = data.Beginning,
                Ending = data.Ending,
                ParkingPlaceId = data.ParkingPlaceId,
                UserId = data.User.Id
            };

            var query = from reservation in context.Reservations
                        where reservation.ParkingPlace.ID == data.ParkingPlaceId
                        && data.Beginning.CompareTo(reservation.Beginning) >= 0 && data.Beginning.CompareTo(reservation.Ending) <= 0
                        && data.Ending.CompareTo(reservation.Beginning) >= 0 && data.Ending.CompareTo(reservation.Ending) <= 0
                        select reservation;

            var query2 = from parkingplace in context.ParkingPlaces
                         where parkingplace.DisabledParking == true
                         && parkingplace.ID == parkingplace.ID
                         select parkingplace;


            var query3 = (from reservation in context.Reservations
                          where reservation.User.Id == data.User.Id
                          select (reservation.Ending - reservation.Beginning).TotalHours)
                         .Sum();

            var entities = await query.ToListAsync();
            var disabled = await query2.FirstOrDefaultAsync();
            if (disabled != null)
            {
                if (entities.Count == 0 && data.User.Disabled == true && ((data.Ending - data.Beginning).TotalHours + query3) <= 12)
                {
                   return true;
                }
            }
            else
            {
                if (entities.Count == 0 && ((data.Ending - data.Beginning).TotalHours + query3) <= 12)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
