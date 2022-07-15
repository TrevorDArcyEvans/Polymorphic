namespace Polymorphic.Models;

public abstract class WeatherForecast
{
  public DateTime Date { get; set; }
  public string? Summary { get; set; }
  public WeatherForecast? Previous { get; set; }
  public int TemperatureC { get; set; }
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
