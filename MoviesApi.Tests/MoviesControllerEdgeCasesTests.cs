using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.Dto;
using Xunit;

namespace MoviesApi.Tests;

public class MoviesControllerEdgeCasesTests : IClassFixture<WebApplicationFactory<MoviesApi.Program>>
{
    private readonly WebApplicationFactory<MoviesApi.Program> _factory;

    public MoviesControllerEdgeCasesTests(WebApplicationFactory<MoviesApi.Program> factory)
    {
        var uniqueDbName = "EdgeCaseTest_MoviesDb_" + Guid.NewGuid().ToString("N");
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("InMemoryDbName", uniqueDbName);
            builder.UseSetting("EnableHttpsRedirect", "false");
            builder.UseSetting("environment", "Development");
        });

        // Ensure DB created
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<MoviesApi.Models.MovieContext>();
        db.Database.EnsureCreated();
    }

    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        // Missing required Title
        var create = new CreateMovieDto { Title = "", Director = "Director", ReleaseYear = 2000, Genre = "Drama" };
        var resp = await client.PostAsJsonAsync("/Movies", create);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
    }

    [Fact]
    public async Task Update_NonExistentMovie_ReturnsNotFound()
    {
        var client = _factory.CreateClient();

        var update = new UpdateMovieDto { Id = 999999, Title = "Nope", Director = "Nobody", ReleaseYear = 2001, Genre = "None" };
        var resp = await client.PutAsJsonAsync($"/Movies/{update.Id}", update);

        Assert.Equal(System.Net.HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Fact]
    public async Task Create_BoundaryValues_Succeeds()
    {
        var client = _factory.CreateClient();

        // ReleaseYear lower bound and long title
        var longTitle = new string('A', 512);
        var create = new CreateMovieDto { Title = longTitle, Director = "Director", ReleaseYear = 1888, Genre = "Experimental" };
        var resp = await client.PostAsJsonAsync("/Movies", create);

        Assert.Equal(System.Net.HttpStatusCode.Created, resp.StatusCode);

        var created = await resp.Content.ReadFromJsonAsync<MovieDto>();
        Assert.NotNull(created);
        Assert.Equal(1888, created!.ReleaseYear);
        Assert.Equal(longTitle, created.Title);
    }
}
