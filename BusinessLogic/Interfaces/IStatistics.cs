using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IStatistics
    {
        Task<List<HourStat>> getUsageRatio(DateOnly date);

        Task<double> getDailyUsageRatio(DateOnly date);

        Task<int> getAllSpaceNumber();

        Task<int> getWeeklyFailedReservationCount(DateOnly beginning);

        Task<double> getWeeklyAverageHours(DateOnly beginning);

    }
}
