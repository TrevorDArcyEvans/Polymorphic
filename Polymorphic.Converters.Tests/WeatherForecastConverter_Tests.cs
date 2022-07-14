namespace Polymorphic.Converters.Tests;

using System.Text.Json;
using FluentAssertions;
using Polymorphic.Models;

[TestFixture]
public sealed class WeatherForecastConverter_Tests
{
  [Test]
  public void RoundTrip_Succeeds()
  {
    var data = new WeatherForecastRH
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
    var options = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
      Converters = { new WeatherForecastConverter() }
    };
    var json = JsonSerializer.Serialize(data, options);

    var instance = JsonSerializer.Deserialize<WeatherForecast>(json, options);

    data.Should().BeEquivalentTo(instance);
  }
}
