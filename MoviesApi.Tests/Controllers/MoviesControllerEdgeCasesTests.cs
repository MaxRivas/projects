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
