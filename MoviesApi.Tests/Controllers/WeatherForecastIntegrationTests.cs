using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;

namespace MoviesApi.Tests.Controllers;

public class WeatherForecastIntegrationTests : IClassFixture<WebApplicationFactory<MoviesApi.Program>>
{
    private readonly WebApplicationFactory<MoviesApi.Program> _factory;

    public WeatherForecastIntegrationTests(WebApplicationFactory<MoviesApi.Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseSetting("environment", "Development"));
    }

    [Fact]
    public async Task Get_Works()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/WeatherForecast");
    resp.EnsureSuccessStatusCode();
    var items = await resp.Content.ReadFromJsonAsync<IEnumerable<object>>();
    Assert.NotNull(items);
    Assert.True(items!.Any());
    }
}
