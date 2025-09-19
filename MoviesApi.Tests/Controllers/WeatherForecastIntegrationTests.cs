using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;

namespace MoviesApi.Tests.Controllers;

/// <summary>
/// Simple smoke test for the scaffolded WeatherForecast controller.
/// This confirms the app and routing are functioning in the test-host.
/// </summary>
public class WeatherForecastIntegrationTests : IClassFixture<WebApplicationFactory<MoviesApi.Program>>
{
    private readonly WebApplicationFactory<MoviesApi.Program> _factory;

    public WeatherForecastIntegrationTests(WebApplicationFactory<MoviesApi.Program> factory)
    {
        // Ensure development environment so Swagger/OpenAPI remains available if needed
        _factory = factory.WithWebHostBuilder(builder => builder.UseSetting("environment", "Development"));
    }

    /// <summary>
    /// Basic GET smoke test: request the endpoint and assert a non-empty response collection.
    /// </summary>
    [Fact]
    public async Task Get_Works()
    {
        var client = _factory.CreateClient();

        // Act: call the endpoint
        var resp = await client.GetAsync("/WeatherForecast");
        resp.EnsureSuccessStatusCode();

        // Assert: response contains at least one item
        var items = await resp.Content.ReadFromJsonAsync<IEnumerable<object>>();
        Assert.NotNull(items);
        Assert.True(items!.Any());
    }
}
