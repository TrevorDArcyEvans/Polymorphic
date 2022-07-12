namespace Polymorphic.Examples;

using Swashbuckle.AspNetCore.Filters;

public class WeatherForecastExample : IExamplesProvider<WeatherForecast>
{
  public WeatherForecast GetExamples()
  {
    return new WeatherForecastEx
    {
      Date = DateTime.UtcNow,
      TemperatureC = Random.Shared.Next(-40, 40),
      Summary = "Fine, winds light to variable",
      RelativeHumidity = Random.Shared.Next(0, 100),
      UV = Random.Shared.Next(0, 10)
    };
  }
}
