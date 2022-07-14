namespace Polymorphic.Converters.Tests;

using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Polymorphic.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

[TestFixture]
public sealed class WeatherForecastConverter_Tests
{
  private readonly JsonSerializerSettings _NS_settings = new JsonSerializerSettings()
  {
    Formatting = Formatting.Indented,
    TypeNameHandling = TypeNameHandling.All
  };

  private readonly JsonSerializerOptions _STJ_options = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    Converters = { new WeatherForecastConverter() }
  };

  [Test]
  public void RoundTrip_Succeeds()
  {
    var data = CreateData();
    var json = JsonSerializer.Serialize(data, _STJ_options);

    var instance = JsonSerializer.Deserialize<WeatherForecast>(json, _STJ_options);

    data.Should().BeEquivalentTo(instance);
  }

  [Test]
  public void Serialise_Compatible_With_Newtonsoft()
  {
    var data = CreateData();
    var json = JsonSerializer.Serialize(data, _STJ_options);

    var instance = JsonConvert.DeserializeObject<WeatherForecast>(json, _NS_settings);

    data.Should().BeEquivalentTo(instance);
  }

  [Test]
  public void Deserialise_Compatible_With_Newtonsoft()
  {
    var data = CreateData();
    var json = JsonConvert.SerializeObject(data, _NS_settings);

    var instance = JsonSerializer.Deserialize<WeatherForecast>(json, _STJ_options);

    data.Should().BeEquivalentTo(instance);
  }

  private static WeatherForecastRH CreateData()
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
    return data;
  }
}
