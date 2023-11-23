namespace DataAccess.Interfaces
{
    public interface IStatistics
    {
        List<float> getUsageRatio(DateOnly date);

        float getDailyUsageRatio(DateOnly date);

        int getAllSpaceNumber();

        int getWeeklyFailedReservationCount(DateOnly beginning);

        int getWeeklyAverageHours(DateOnly beginning);

    }
}
