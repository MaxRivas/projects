using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MoviesApi.Tests;

public class WeatherForecastIntegrationTests : IClassFixture<WebApplicationFactory<MoviesApi.Program>>
{
    private readonly WebApplicationFactory<MoviesApi.Program> _factory;

    public WeatherForecastIntegrationTests(WebApplicationFactory<MoviesApi.Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessAndNonEmpty()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/WeatherForecast");
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<object[]>();
        Assert.NotNull(items);
        Assert.NotEmpty(items!);
    }
}
