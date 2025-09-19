namespace MoviesApi;

/// <summary>
/// Simple model used by the <see cref="Controllers.WeatherForecastController"/> sample.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// The date of the forecast.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Temperature in Celsius.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Temperature in Fahrenheit (calculated from <see cref="TemperatureC"/>).
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Human-readable summary.
    /// </summary>
    public string? Summary { get; set; }
}