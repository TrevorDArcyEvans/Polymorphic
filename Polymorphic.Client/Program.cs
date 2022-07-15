using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Polymorphic.Converters;
using Polymorphic.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

// https endpoint does not work on Linux
const string WeatherForecastUri = "http://localhost:5211/WeatherForecast";

var client = new HttpClient();

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

#region Newtonsoft.Json

{
  var settings = new JsonSerializerSettings
  {
    Formatting = Formatting.Indented,
    TypeNameHandling = TypeNameHandling.All
  };
  var json = JsonConvert.SerializeObject(data, settings);

  Console.WriteLine($"Newtonsoft.Json:");
  Console.WriteLine($"{json}");

  try
  {
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var resp = await client.PostAsync(WeatherForecastUri, content);
    var msg = await resp.Content.ReadAsStringAsync();
    Console.WriteLine($"{msg}");
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Ex: {ex.Message}");
  }
}

#endregion

Console.WriteLine();

#region System.Text.Json

{
  var options = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    Converters = { new WeatherForecastConverter() }
  };
  var json = JsonSerializer.Serialize(data, options);

  var instance = JsonSerializer.Deserialize<WeatherForecast>(json, options);

  Console.WriteLine($"System.Text.Json:");
  Console.WriteLine($"{json}");

  try
  {
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var resp = await client.PostAsync(WeatherForecastUri, content);
    var msg = await resp.Content.ReadAsStringAsync();
    Console.WriteLine($"{msg}");
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Ex: {ex.Message}");
  }
}

#endregion

/*
Default serialisation & results:


Newtonsoft.Json:
{
  "$type": "Polymorphic.Models.WeatherForecastRH, Polymorphic.Models",
  "RelativeHumidity": 47,
  "TemperatureC": 13,
  "TemperatureF": 55,
  "Date": "2022-07-14T09:38:16.1820354Z",
  "Summary": "Fine, winds light to variable",
  "Previous": {
    "$type": "Polymorphic.Models.WeatherForecastRH, Polymorphic.Models",
    "RelativeHumidity": 45,
    "TemperatureC": -31,
    "TemperatureF": -23,
    "Date": "2022-07-13T09:38:16.1837775Z",
    "Summary": "Cloudy with a chance of meatballs",
    "Previous": null
  }
}
Deserialised Polymorphic.Models.WeatherForecastRH

System.Text.Json:
{
  "relativeHumidity": 47,
  "temperatureC": 13,
  "temperatureF": 55,
  "date": "2022-07-14T09:38:16.1820354Z",
  "summary": "Fine, winds light to variable",
  "previous": {
    "date": "2022-07-13T09:38:16.1837775Z",
    "summary": "Cloudy with a chance of meatballs",
    "previous": null
  }
}
{"$type":"Microsoft.AspNetCore.Mvc.NewtonsoftJson.ValidationProblemDetailsConverter+AnnotatedValidationProblemDetails, Microsoft.AspNetCore.Mvc.NewtonsoftJson","errors":{"$type":"System.Collections.Generic.Dictionary`2[[System.String, System.Private.CoreLib],[System.String[], System.Private.CoreLib]], System
.Private.CoreLib","item":{"$type":"System.String[], System.Private.CoreLib","$values":["The item field is required."]},"relativeHumidity":{"$type":"System.String[], System.Private.CoreLib","$values":["Could not create an instance of type Polymorphic.Models.WeatherForecast. Type is an interface or abstract cl
ass and cannot be instantiated. Path 'relativeHumidity', line 2, position 21."]}},"type":"https://tools.ietf.org/html/rfc7231#section-6.5.1","title":"One or more validation errors occurred.","status":400,"traceId":"00-7f442902f2adb894dedcd32c2476c3f2-be20a1f2f2bb9dc5-00"}

*/
