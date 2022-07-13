namespace Polymorphic.Examples;

using Swashbuckle.AspNetCore.Filters;

public class WeatherForecastExample : IExamplesProvider<WeatherForecast>
{
  public WeatherForecast GetExamples()
  {
    return new WeatherForecastRH
    {
      Date = DateTime.UtcNow,
      TemperatureC = Random.Shared.Next(-40, 40),
      Summary = "Fine, winds light to variable",
      RelativeHumidity = Random.Shared.Next(0, 100),
      Previous = new WeatherForecastRH
      {
        Date = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
        TemperatureC = Random.Shared.Next(-40, 40),
        Summary = "Cloudy with a chance of meatballs",
        RelativeHumidity = Random.Shared.Next(0, 100),
        Previous = null
      }
    };
  }
}
