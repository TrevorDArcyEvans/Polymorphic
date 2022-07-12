namespace Polymorphic;

public abstract class WeatherForecast
{
  public DateTime Date { get; set; }

  public int TemperatureC { get; set; }

  public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

  public string? Summary { get; set; }
}

public class WeatherForecastEx : WeatherForecast
{
  public int RelativeHumidity { get; set; }
  public int UV { get; set; }
}
