using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IStatistics
    {
        Task<List<HourStat>> getUsageRatio(DateOnly date);

        Task<double> getDailyUsageRatio(DateOnly date);

        Task<int> getAllSpaceNumber();

        Task<int> getWeeklyFailedReservationCount(DateTime beginning);

        Task<double> getWeeklyAverageHours(DateTime beginning);

        Task<double> GetAverageNumOfDays(DateTime referenceEnd);
    }
}
