﻿namespace Polymorphic.Server.Models;

public abstract class Forecast
{
  public DateTime Date { get; set; }

  public string? Summary { get; set; }

  public Forecast? Previous { get; set; }
}
