using Polymorphic.Models;

namespace Polymorphic.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
  private static readonly string[] Summaries = new[]
  {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  private readonly ILogger<WeatherForecastController> _logger;

  public WeatherForecastController(ILogger<WeatherForecastController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  public ActionResult<IEnumerable<WeatherForecast>> Get()
  {
    return new WeatherForecast[]
    {
      new WeatherForecastRH
      {
        Date = DateTime.UtcNow.AddDays(1),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        RelativeHumidity = Random.Shared.Next(0, 100)
      },
      new WeatherForecastPollen
      {
        Date = DateTime.UtcNow.AddDays(1),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        Count = Random.Shared.Next(0, 100)
      },
      new WeatherForecastUV
      {
        Date = DateTime.UtcNow.AddDays(1),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        UV = Random.Shared.Next(0, 100)
      },
      new WeatherForecastWind()
      {
        Date = DateTime.UtcNow.AddDays(1),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        Speed = Random.Shared.Next(0, 100),
        Direction = Random.Shared.Next(0, 360)
      }
    };
  }

  [HttpPost]
  public ActionResult Add([FromBody] WeatherForecast item)
  {
    return Ok($"Deserialised {item.GetType().FullName}");
  }
}
