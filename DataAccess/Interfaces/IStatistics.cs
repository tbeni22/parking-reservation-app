namespace DataAccess.Interfaces
{
    public interface IStatistics
    {
        List<double> getUsageRatio(DateOnly date);

        double getDailyUsageRatio(DateOnly date);

        int getAllSpaceNumber();

        int getWeeklyFailedReservationCount(DateOnly beginning);

        double getWeeklyAverageHours(DateOnly beginning);

    }
}
