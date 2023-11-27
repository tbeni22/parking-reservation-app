using BusinessLogic.DTOs;
using DataAccess;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces.Implementations
{
    public class StatisticsService : IStatistics
    {
        private ParkingContext _context;

        public StatisticsService(ParkingContext context) {

            _context = context;
        }
        public async Task<int> getAllSpaceNumber()
        {
            return await _context.ParkingPlaces.CountAsync();
        }

        public async Task<double> getDailyUsageRatio(DateOnly date)
        {
            var query = from reservation in _context.Reservations
                         where DateOnly.FromDateTime(reservation.Beginning).Equals(date)
                         select new { reservation.Beginning.Hour, reservation.ID } into reservationHour
                         group reservationHour by new { reservationHour.Hour } into reservationGroups
                         select new
                         {
                             Count = reservationGroups.Select(x => x.ID).Count()
                         };

            int spaceCount = await getAllSpaceNumber();

            try
            {
                return await query.AverageAsync(x => x.Count / spaceCount);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<HourStat>> getUsageRatio(DateOnly date)
        {
            int spaceNumber = await getAllSpaceNumber();

            var query = from reservation in _context.Reservations
                        where DateOnly.FromDateTime(reservation.Beginning).Equals(date)
                        group reservation by reservation.Beginning.Hour into reservationGroups
                        select new HourStat
                        {
                            Hour = reservationGroups.Key,
                            Ratio = (double)(reservationGroups.Select(x => x.ID).Count()) / (double)spaceNumber
                        };
            return await query.ToListAsync();
        }

        // 
        public async Task<double> getWeeklyAverageHours(DateTime beginning)
        {

            var query = from reservation in _context.Reservations
                        where reservation.Beginning.Date.CompareTo(beginning) >= 0
                        && reservation.Beginning.Date.CompareTo(beginning.AddDays(7)) < 0
                        select new { reservation.UserId, Time = reservation.Ending - reservation.Beginning };
                       /* group weeklyreservations by new { weeklyreservations.UserId } into usersWeekly
                        select new
                        {
                            UserID = usersWeekly.Select(x => x.UserId),
                            Time = usersWeekly.Select(x => x.Time.Ticks).Average()
                        };
                       */
            var list = await query.ToListAsync();

            var query2 = from weeklyReservation in list
                         select weeklyReservation into weeklyreservations
                         group weeklyreservations by new { weeklyreservations.UserId } into userWeekly
                         select new
                         {
                             UserID = userWeekly.Select(x => x.UserId),
                             Time = userWeekly.Select(x => x.Time.Ticks).Average()
                         };

            int userCount = await _context.User.CountAsync();

            try
            {

                return query2.Select(x => x.Time).Sum() / userCount;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<double> GetAverageNumOfDays(DateTime referenceEnd)
        {
            var referenceStart = referenceEnd.AddDays(-6);
            //var start = referenceStart.DayOfYear + referenceStart.Year * 366;
            //var end = referenceEnd.DayOfYear + referenceEnd.Year * 366;

            var query = from reservation in _context.Reservations
                        where reservation.Beginning.Date.CompareTo(referenceStart) >= 0
                        && reservation.Beginning.Date.CompareTo(referenceEnd) <= 0
                        group reservation by new { reservation.UserId } into userReservations
                        select new
                        {
                            UserId = userReservations.Key.UserId,
                            Days = userReservations.Select(x => x.Beginning.DayOfYear).Count()
                        };

            try
            {

                return await query.Select(x => x.Days).DefaultIfEmpty().AverageAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> getWeeklyFailedReservationCount(DateTime beginning)
        {
            var end = beginning.AddDays(6);

            var query = from reservation in _context.FailureReports
                        where reservation.Beginning.Date.CompareTo(beginning) >= 0
                        && reservation.Beginning.Date.CompareTo(end) <= 0
                        
                        select reservation;

                return await query.CountAsync();
        }
    }
}
