using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces.Implementations
{
    public class StatisticsService : IStatistics
    {

        private ParkingContext _context;

        public StatisticsService(ParkingContext context) {
            _context = context;
        }
        public int getAllSpaceNumber()
        {
            return _context.ParkingPlaces.Count();
        }

        public double getDailyUsageRatio(DateOnly date)
        {
            var query = from reservation in _context.Reservations
                         where DateOnly.FromDateTime(reservation.Beginning).Equals(date)
                         select new { reservation.Beginning.Hour, reservation.ID } into reservationHour
                         group reservationHour by new { reservationHour.Hour } into reservationGroups
                         select new
                         {
                             Count = reservationGroups.Select(x => x.ID).Count()
                         };

            return query.Average(x => x.Count/getAllSpaceNumber());
        }

        public List<double> getUsageRatio(DateOnly date)
        {
           var query = from reservation in _context.Reservations
                        where DateOnly.FromDateTime(reservation.Beginning).Equals(date)
                        select new { reservation.Beginning.Hour, reservation.ID } into reservationHour
                        group reservationHour by new { reservationHour.Hour } into reservationGroups
                        select new
                        {

                            Average = (double)(reservationGroups.Select(x => x.ID).Count()) / (double)getAllSpaceNumber(),
                        };
            return query.Select(a => a.Average).ToList();
        }

        public double getWeeklyAverageHours(DateOnly beginning)
        {

            var query = from reservation in _context.Reservations
                        where (DateOnly.FromDateTime(reservation.Beginning).DayNumber >= beginning.DayNumber
                        && DateOnly.FromDateTime(reservation.Beginning).DayNumber <= beginning.AddDays(7).DayNumber)
                        select new { reservation.UserId, Time = reservation.Ending - reservation.Beginning } into weeklyreservations
                        group weeklyreservations by new { weeklyreservations.UserId } into usersWeekly
                        select new
                        {
                            UserID = usersWeekly.Select(x => x.UserId),
                            Time = usersWeekly.Select(x => x.Time.Ticks).Average()
                        };

            return query.Select(x => x.Time).Sum()/_context.User.Count();
        }

        public int getWeeklyFailedReservationCount(DateOnly beginning)
        {
            var query = from reservation in _context.FailureReports
                        where (DateOnly.FromDateTime(reservation.Beginning).DayNumber >= beginning.DayNumber
                        && DateOnly.FromDateTime(reservation.Beginning).DayNumber <= beginning.AddDays(7).DayNumber)
                        select reservation;

            return query.Count();

        }
    }
}
