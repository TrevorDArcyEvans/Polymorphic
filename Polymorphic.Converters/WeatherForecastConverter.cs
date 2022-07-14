namespace Polymorphic.Converters;

using System.Text.Json;
using Newtonsoft.Json;
using Polymorphic.Models;

public sealed class WeatherForecastConverter : System.Text.Json.Serialization.JsonConverter<WeatherForecast>
{
  private readonly JsonSerializerSettings _settings = new()
  {
    Formatting = Formatting.Indented,
    TypeNameHandling = TypeNameHandling.All
  };

  public override bool CanConvert(Type typeToConvert)
  {
    var retval = typeof(WeatherForecast).IsAssignableFrom(typeToConvert);
    return retval;
  }

  public override WeatherForecast? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jdoc = JsonDocument.ParseValue(ref reader);
    var json = jdoc.RootElement.ToString();

    // use other deserialiser to create object
    var instance = JsonConvert.DeserializeObject<WeatherForecast>(json, _settings);
    return instance;
  }

  public override void Write(Utf8JsonWriter writer, WeatherForecast value, JsonSerializerOptions options)
  {
    // use other serialiser to generate json
    var json = JsonConvert.SerializeObject(value, _settings);

    // json is json, so we can write it to System.Text.Json serialiser
    writer.WriteRawValue(json);
  }
}
