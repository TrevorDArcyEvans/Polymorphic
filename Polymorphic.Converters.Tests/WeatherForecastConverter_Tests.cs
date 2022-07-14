namespace Polymorphic.Converters.Tests;

using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Polymorphic.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

[TestFixture]
public sealed class WeatherForecastConverter_Tests
{
  [Test]
  public void RoundTrip_Succeeds()
  {
    var data = CreateData();
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

  [Test]
  public void Serialise_Compatible_With_Newtonsoft()
  {
    var data = CreateData();
    var options = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
      Converters = { new WeatherForecastConverter() }
    };
    var json = JsonSerializer.Serialize(data, options);

    var settings = new JsonSerializerSettings()
    {
      Formatting = Formatting.Indented,
      TypeNameHandling = TypeNameHandling.All
    };
    var instance = JsonConvert.DeserializeObject<WeatherForecast>(json, settings);

    data.Should().BeEquivalentTo(instance);
  }

  [Test]
  public void Deserialise_Compatible_With_Newtonsoft()
  {
    var data = CreateData();
    var settings = new JsonSerializerSettings()
    {
      Formatting = Formatting.Indented,
      TypeNameHandling = TypeNameHandling.All
    };
    var json = JsonConvert.SerializeObject(data, settings);

    var options = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
      Converters = { new WeatherForecastConverter() }
    };
    var instance = JsonSerializer.Deserialize<WeatherForecast>(json, options);

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
