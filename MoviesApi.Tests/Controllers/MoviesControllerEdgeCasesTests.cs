using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.Models;
using MoviesApi.Dto;
using Xunit;

namespace MoviesApi.Tests.Controllers;

/// <summary>
/// Edge-case integration tests for <see cref="MoviesApi.Controllers.MoviesController"/>.
/// These tests validate invalid input and non-existent resource behaviours.
/// They are intentionally minimal and rely on the same test-host pattern as other integration tests.
/// </summary>
public class MoviesControllerEdgeCasesTests : IClassFixture<WebApplicationFactory<MoviesApi.Program>>
{
    private readonly WebApplicationFactory<MoviesApi.Program> _factory;

    public MoviesControllerEdgeCasesTests(WebApplicationFactory<MoviesApi.Program> factory)
    {
        // Use a unique in-memory DB name per test class to isolate state between runs.
        var uniqueDbName = "EdgeCaseTest_MoviesDb_" + Guid.NewGuid().ToString("N");
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("InMemoryDbName", uniqueDbName);
            // Disable HTTPS redirect to reduce test log noise
            builder.UseSetting("EnableHttpsRedirect", "false");
        });

        // Ensure the in-memory database is created prior to running tests
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
            db.Database.EnsureCreated();
        }
    }

    /// <summary>
    /// Sending an invalid create model (required fields missing or out-of-range) should return BadRequest.
    /// This asserts model validation is wired and that controller responds with 400 for invalid payloads.
    /// </summary>
    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        // Create payload intentionally invalid: empty strings and too-small ReleaseYear
    var create = new CreateMovieDto { Title = string.Empty, Director = string.Empty, ReleaseYear = 1800, Genre = string.Empty };
        var resp = await client.PostAsJsonAsync("/Movies", create);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
    }

    /// <summary>
    /// Updating a non-existent movie must return NotFound (404).
    /// Verifies controller handles missing resources gracefully.
    /// </summary>
    [Fact]
    public async Task Update_Nonexistent_ReturnsNotFound()
    {
        var client = _factory.CreateClient();

        var update = new UpdateMovieDto { Id = 9999, Title = "Doesn't Exist", Director = "None", ReleaseYear = 2000, Genre = "None" };
        var resp = await client.PutAsJsonAsync($"/Movies/{update.Id}", update);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, resp.StatusCode);
    }

    /// <summary>
    /// Patching with an invalid ReleaseYear should return BadRequest; controller validates fields provided in Patch DTO.
    /// </summary>
    [Fact]
    public async Task Patch_InvalidReleaseYear_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        // Create a valid movie first, then attempt an invalid partial update
    var create = new CreateMovieDto { Title = "Patch Test", Director = "Dir", ReleaseYear = 2020, Genre = "Drama" };
        var createResp = await client.PostAsJsonAsync("/Movies", create);
        var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

        var patch = new PatchMovieDto { ReleaseYear = 1400 };
        var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, patchResp.StatusCode);
    }

    /// <summary>
    /// Create a movie with boundary values (very long title and lower ReleaseYear bound) to exercise edge conditions.
    /// </summary>
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

    /// <summary>
    /// Patch should only update the fields provided in the Patch DTO and leave other fields unchanged.
    /// </summary>
    [Fact]
    public async Task Patch_PartialUpdate_OnlyUpdatesSpecifiedFields()
    {
        var client = _factory.CreateClient();

        // Create an initial movie
        var create = new CreateMovieDto { Title = "PatchTest", Director = "First", ReleaseYear = 2000, Genre = "Test" };
        var createResp = await client.PostAsJsonAsync("/Movies", create);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

        // Patch only the Title
        var patch = new PatchMovieDto { Title = "Patched" };
        var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, patchResp.StatusCode);

        var got = await client.GetFromJsonAsync<MovieDto>($"/Movies/{created.Id}");
        Assert.Equal("Patched", got!.Title);
        Assert.Equal("First", got.Director); // unchanged
    }

    /// <summary>
    /// Re-assert invalid patch handling with different bad year to ensure validation is consistent.
    /// </summary>
    [Fact]
    public async Task Patch_InvalidReleaseYear_ReturnsBadRequest_DifferentCase()
    {
        var client = _factory.CreateClient();

        var create = new CreateMovieDto { Title = "PatchInvalid", Director = "Dev", ReleaseYear = 2005, Genre = "Test" };
        var createResp = await client.PostAsJsonAsync("/Movies", create);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

        var patch = new PatchMovieDto { ReleaseYear = 1700 };
        var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, patchResp.StatusCode);
    }
}
namespace MoviesApi.Tests.Controllers;

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
            // Disable HTTPS redirect to reduce test log noise
            builder.UseSetting("EnableHttpsRedirect", "false");
            // Use a unique in-memory DB name per test class to isolate state between runs
        });

        // Ensure the in-memory database is created prior to running tests
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
            db.Database.EnsureCreated();
        }
    }

    /// <summary>
    /// Edge-case integration tests for <see cref="MoviesApi.Controllers.MoviesController"/>.
    /// These tests validate invalid input and non-existent resource behaviours.
    /// They are intentionally minimal and rely on the same test-host pattern as other integration tests.
    /// </summary>
    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        // Create payload intentionally invalid: empty strings and too-small ReleaseYear
    var create = new CreateMovieDto { Title = string.Empty, Director = string.Empty, ReleaseYear = 1800, Genre = string.Empty };
        var resp = await client.PostAsJsonAsync("/Movies", create);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
    }

    /// <summary>
    /// Updating a non-existent movie must return NotFound (404).
    /// Verifies controller handles missing resources gracefully.
    /// </summary>
    [Fact]
    public async Task Update_Nonexistent_ReturnsNotFound()
    {
        var client = _factory.CreateClient();

        var update = new UpdateMovieDto { Id = 9999, Title = "Doesn't Exist", Director = "None", ReleaseYear = 2000, Genre = "None" };
        var resp = await client.PutAsJsonAsync($"/Movies/{update.Id}", update);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, resp.StatusCode);
    }

    /// <summary>
    /// Patching with an invalid ReleaseYear should return BadRequest; controller validates fields provided in Patch DTO.
    /// </summary>
    [Fact]
    public async Task Patch_InvalidReleaseYear_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        // Create a valid movie first, then attempt an invalid partial update
        var create = new CreateMovieDto { Title = "Patch Test", Director = "Dir", ReleaseYear = 2020, Genre = "Drama" };
        var createResp = await client.PostAsJsonAsync("/Movies", create);
        var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

        var patch = new PatchMovieDto { ReleaseYear = 1400 };
        var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, patchResp.StatusCode);
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

    [Fact]
    public async Task Patch_PartialUpdate_OnlyUpdatesSpecifiedFields()
    {
        var client = _factory.CreateClient();

        // Create an initial movie
        var create = new CreateMovieDto { Title = "PatchTest", Director = "First", ReleaseYear = 2000, Genre = "Test" };
        var createResp = await client.PostAsJsonAsync("/Movies", create);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

        // Patch only the Title
        var patch = new PatchMovieDto { Title = "Patched" };
        var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, patchResp.StatusCode);

        var got = await client.GetFromJsonAsync<MovieDto>($"/Movies/{created.Id}");
        Assert.Equal("Patched", got!.Title);
        Assert.Equal("First", got.Director); // unchanged
    }

    [Fact]
    public async Task Patch_InvalidReleaseYear_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        var create = new CreateMovieDto { Title = "PatchInvalid", Director = "Dev", ReleaseYear = 2005, Genre = "Test" };
        var createResp = await client.PostAsJsonAsync("/Movies", create);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

        var patch = new PatchMovieDto { ReleaseYear = 1700 };
        var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, patchResp.StatusCode);
    }
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
    var create = new CreateMovieDto { Title = string.Empty, Director = "Director", ReleaseYear = 2000, Genre = "Drama" };
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

    [Fact]
    public async Task Patch_PartialUpdate_OnlyUpdatesSpecifiedFields()
    {
        var client = _factory.CreateClient();

        // Create an initial movie
        var create = new CreateMovieDto { Title = "PatchTest", Director = "First", ReleaseYear = 2000, Genre = "Test" };
        var createResp = await client.PostAsJsonAsync("/Movies", create);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

        // Patch only the Title
        var patch = new PatchMovieDto { Title = "Patched" };
        var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, patchResp.StatusCode);

        var got = await client.GetFromJsonAsync<MovieDto>($"/Movies/{created.Id}");
        Assert.Equal("Patched", got!.Title);
        Assert.Equal("First", got.Director); // unchanged
    }

    [Fact]
    public async Task Patch_InvalidReleaseYear_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        var create = new CreateMovieDto { Title = "PatchInvalid", Director = "Dev", ReleaseYear = 2005, Genre = "Test" };
        var createResp = await client.PostAsJsonAsync("/Movies", create);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<MovieDto>();

        var patch = new PatchMovieDto { ReleaseYear = 1700 };
        var patchResp = await client.PatchAsJsonAsync($"/Movies/{created!.Id}", patch);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, patchResp.StatusCode);
    }
}
