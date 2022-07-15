# Polymorphic JSON Marshalling
**TL;DR** `System.Text.Json` is broken, use `Newtonsoft.Json`

## Background
<details>

The object model is a variation on the standard `WeatherForecast` which is part of the standard
Microsoft .NET Core project template.

```mermaid
classDiagram

class WeatherForecast {
  +DateTime Date
  +string? Summary
  +Forecast? Previous
  +int TemperatureC
  +int TemperatureF
}

class WeatherForecastPollen {
  +int Count
}

class WeatherForecastRH {
  +int RelativeHumidity
}

class WeatherForecastUV {
  +int UV
}

class WeatherForecastWind {
  +int Speed
  +int Direction
}

WeatherForecast "1" --> "0..1" WeatherForecast

WeatherForecast <|-- WeatherForecastPollen
WeatherForecast <|-- WeatherForecastRH
WeatherForecast <|-- WeatherForecastUV
WeatherForecast <|-- WeatherForecastWind

```

The most important difference is the addition of a reference to a previous `WeatherForecast`.

</details>

## Default JSON serialisation
<details>

Newtonsoft.Json:
```json
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
```

System.Text.Json:
```json
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

```

Points to note:
* `Newtonsoft.Json`
  * stores the type in a `$type` metadata tag
  * has correctly serialised top level `WeatherForecast`
  * has correctly serialised reference to previous `WeatherForecast`
* `System.Text.Json`
  * does **not** have any type information
  * has **not** correctly serialised reference to top level `WeatherForecast`
  * has **not** correctly serialised reference to previous `WeatherForecast`

</details>

## Problem
<details>

```mermaid

flowchart LR
  client[client/STJ] -- WeatherForecast <--> server[server/Newtonsoft]

```

When two parties need to send a `WeatherForecast` to each other via JSON, then
both parties need to use `Newtonsoft.Json` otherwise [object slicing](https://en.wikipedia.org/wiki/Object_slicing)
will happen.

In the above scenario, one party is using `System.Text.Json`, so will only
be able to send/receive the base `WeatherForecast`.

</details>

## Solution
<details>

  **This section has been intentionally left blank.**

Seriously, there *are* solutions by writing custom converters working in concert with
type discriminators but these get very complicated, very quickly.

</details>

## Conclusion
<details>

`System.Text.Json` has been designed for speed and it is definitely faster than `Newtonsoft.Json`.
However, in doing so, it has sacrificed completeness; and polymorphic marshalling is just one such area.

At this stage, a hybrid approach is recommended.  Use `Newtonsoft.Json` in the first instance;
profile your code; and use `System.Text.Json` for those sections where JSON manipulation is the
bottleneck.

</details>

## Further information
<details>

`System.Text.Json` does not support polymorphic marshalling (serialisation + deserialisation): 
* [How to serialize properties of derived classes with System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-polymorphism)
* [Support polymorphic deserialization](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-6-0#support-polymorphic-deserialization)

Whereas `Newtonsoft.Json` supports this via `TypeNameHandling.All`, though at the expense of reduced security:
* [Do not use TypeNameHandling values other than None](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2326)
* [TypeNameHandling Enumeration](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_TypeNameHandling.htm)

  _`TypeNameHandling` should be used with caution when your application deserializes JSON from an external source. Incoming types should be validated with a custom SerializationBinder when deserializing with a value other than `None.`_

* [[System.Text.Json] serialize/deserialize any object](https://github.com/dotnet/runtime/issues/30969#issuecomment-535779492)
* [How to configure Json.NET to create a vulnerable web API](https://www.alphabot.com/security/blog/2017/net/How-to-configure-Json.NET-to-create-a-vulnerable-web-API.html)

There are some incomplete workarounds:
* [Polymorphic Deserialization With System.Text.Json in .NET 5.0](https://badecho.com/index.php/2020/12/04/polymorphic-json-deserialization/)

</details>

## Further work
<details>

The current 'solution' is to use `Newtonsoft.Json` under the covers to do marshalling for
`System.Text.Json`.  This is far from ideal and is just using `Newtonsoft.Json` by proxy.
However, there are unit tests provided in the hope that a proper solution, based on
`System.Text.Json`, can be developed.

</details>

