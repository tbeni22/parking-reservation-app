using parking_reservation_app.DAL.ContosoUniversity.DAL;

namespace parking_reservation_app.Data
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastService(ParkingContext context) {
            foreach (var user in context.User)
            {
                Console.WriteLine(user);
            }

            context.User.Add(new User());
            context.SaveChanges();

            foreach(var user in context.User)
                Console.WriteLine(user);

        }

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray());
        }
    }
}