using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers;

[ApiController]
[Route("[controller]")]
/// <summary>
/// Example weather forecast controller provided by the template. Returns a small list of forecasts.
/// </summary>
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    /// <summary>
    /// Creates a new instance of <see cref="WeatherForecastController"/>.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Returns a short list of sample <see cref="WeatherForecast"/> items.
    /// </summary>
    /// <returns>Array of <see cref="WeatherForecast"/>.</returns>
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
