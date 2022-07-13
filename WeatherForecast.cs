namespace Polymorphic;

public abstract class WeatherForecast : Forecast
{
  public int TemperatureC { get; set; }

  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
