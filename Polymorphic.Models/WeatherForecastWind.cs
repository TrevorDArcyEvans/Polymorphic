namespace Polymorphic.Models;

public sealed class WeatherForecastWind : WeatherForecast
{
  public int Speed { get; set; }
  public int Direction { get; set; }
}
