namespace DataAccess.Interfaces
{
    public interface IStatistics
    {
        Task<List<double>> getUsageRatio(DateOnly date);

        Task<double> getDailyUsageRatio(DateOnly date);

        Task<int> getAllSpaceNumber();

        Task<int> getWeeklyFailedReservationCount(DateOnly beginning);

        Task<double> getWeeklyAverageHours(DateOnly beginning);

    }
}
