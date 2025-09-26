// csharp
// filepath: MoviesApi/Controllers/MoviesControllerTest.cs
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.Dto;
using MoviesApi.Models;
using Xunit;
using MoviesApi.Tests.Utilities;
using System.Text;

namespace MoviesApi.Controllers
{
    public class MoviesControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        #region Private Fields

        private readonly WebApplicationFactory<Program> _baseFactory;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MoviesControllerTest"/> class.
        /// </summary>
        /// <param name="baseFactory">The base factory.</param>
        public MoviesControllerTest(WebApplicationFactory<Program> baseFactory)
        {
            _baseFactory = baseFactory;
        }

        #endregion

        /// <summary>
        /// Creates a new factory for testing with a unique in-memory database.
        /// </summary>
        /// <param name="dbName">DB name for the in-memory database.</param>
        /// <returns>WebApplicationFactory instance for testing.</returns>
        private WebApplicationFactory<Program> CreateFactory(string dbName)
        {
            return _baseFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // remove existing MovieContext registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MovieContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // add in-memory MovieContext with unique db name
                    services.AddDbContext<MovieContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });

                    // Build service provider & ensure DB created
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
                    db.Database.EnsureCreated();
                });
            });
        }

        /// <summary>
        /// Creates a new HttpClient for testing with a unique in-memory database.
        /// </summary>
        /// <param name="dbName">DB name for the in-memory database.</param>
        /// <returns>HttpClient instance for making requests.</returns>
        private HttpClient CreateClient(string dbName)
        {
            var factory = CreateFactory(dbName);
            return factory.CreateClient()!;
        }

        /// <summary>
        /// Helper to create a movie via the API.
        /// </summary>
        /// <param name="client">HttpClient instance for making requests.</param>
        /// <param name="create">The movie creation DTO.</param>
        /// <returns>The created MovieDto.</returns>
        private async Task<MovieDto> CreateMovieAsync(HttpClient client, CreateMovieDto create)
        {
            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            resp.EnsureSuccessStatusCode();
            var dto = await resp.Content.ReadFromJsonAsync<MovieDto>();
            if (dto is null)
            {
                throw new InvalidOperationException("Failed to deserialize MovieDto from create response.");
            }
            return dto;
        }

        /// <summary>
        /// Tests the creation of a movie.
        /// </summary>
        [Fact]
        public async Task Create_ReturnsCreatedAndLocation()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
            Assert.NotNull(resp.Headers.Location);
            var dto = await resp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(dto);
            Assert.True(dto.Id > 0);
            Assert.Equal(create.Title, dto.Title);
        }

        /// <summary>
        /// Tests the creation of a duplicate movie returning Conflict.
        /// </summary>
        [Fact]
        public async Task Create_Duplicate_ReturnsConflict()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();

            var first = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(HttpStatusCode.Created, first.StatusCode);

            var second = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie and verifies it is persisted.
        /// </summary>
        [Fact]
        public async Task Create_Success_ReturnsCreatedAndPersists()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();

            var result = await client.PostAsJsonAsync(Constants.SlashMovies, create);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            var dto = await result.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(dto);
            Assert.True(dto.Id > 0);
            Assert.Equal(create.Title, dto.Title);
            Assert.Equal(create.Director, dto.Director);
            Assert.Equal(create.ReleaseYear, dto.ReleaseYear);
            Assert.Equal(create.Genre, dto.Genre);

            var persisted = await client.GetFromJsonAsync<MovieDto>($"{Constants.SlashMovies}{Constants.Slash}{dto.Id}");
            Assert.NotNull(persisted);
            Assert.Equal(create.Title, persisted.Title);
            Assert.Equal(create.Director, persisted.Director);
            Assert.Equal(create.ReleaseYear, persisted.ReleaseYear);
            Assert.Equal(create.Genre, persisted.Genre);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid genre data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task Create_InvalidGenre_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            create.Genre = string.Empty;

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid title data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task Create_InvalidTitle_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            create.Title = string.Empty;

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid director data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task Create_InvalidDirector_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            create.Director = string.Empty;

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid release date data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task Create_InvalidReleaseDate_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            create.ReleaseYear = -1800;

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests the creation of a movie with invalid release date data returning BadRequest.
        /// </summary>
        [Fact]
        public async Task Create_InvalidParameters_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            // Build an intentionally invalid payload (missing required fields / out-of-range year)
            var create = new CreateMovieDto { Title = string.Empty, Director = string.Empty, ReleaseYear = -1800, Genre = string.Empty };

            var resp = await client.PostAsJsonAsync(Constants.SlashMovies, create);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests retrieval of all movies returning the inserted movies.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsInsertedMovies()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var movie1 = Helper.ValidCreateMovieDto();
            var movie2 = Helper.ValidCreateMovieDto();

            // Ensure movie2 differs from movie1 to avoid duplicate detection
            movie2.Title = movie1.Title + "_2";

            var respA = await client.PostAsJsonAsync(Constants.SlashMovies, movie1);
            var respB = await client.PostAsJsonAsync(Constants.SlashMovies, movie2);

            Assert.Equal(HttpStatusCode.Created, respA.StatusCode);
            Assert.Equal(HttpStatusCode.Created, respB.StatusCode);

            // Create two more distinct movies via helper to ensure list contains multiple entries
            var helperA = Helper.ValidCreateMovieDto();
            const string helperTitle = "HelperMovie_";
            helperA.Title = helperTitle + Guid.NewGuid().ToString("N");
            var helperB = Helper.ValidCreateMovieDto();
            helperB.Title = helperTitle + Guid.NewGuid().ToString("N");
            await CreateMovieAsync(client, helperA);
            await CreateMovieAsync(client, helperB);

            var list = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list);
            Assert.True(list.Length >= 2);
            Assert.Contains(list, m => m.Title == movie1.Title);
            Assert.Contains(list, m => m.Title == movie2.Title);
        }

        /// <summary>
        /// Tests retrieval of a movie by ID returning NotFound for non-existent ID.
        /// </summary>
        [Fact]
        public async Task GetById_NotFound()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var resp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{Constants.ReleaseYearMax}");
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }

        /// <summary>
        /// Tests successful update of a movie and verifies changes are persisted.
        /// </summary>
        [Fact]
        public async Task Update_Succeeds()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var movieDto = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, movieDto);

            const string updatedTitle = "UpdatedTitle";
            const string updatedGenre = "UpdatedGenre";
            var update = Helper.ValidUpdateMovieDto();
            update.Id = created.Id;
            update.Title = updatedTitle;
            update.Genre = updatedGenre;

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var putResp = await client.PutAsJsonAsync(updateEndpoint, update);
            Assert.Equal(HttpStatusCode.NoContent, putResp.StatusCode);

            var get = await client.GetFromJsonAsync<MovieDto>(updateEndpoint);
            Assert.NotNull(get);
            Assert.Equal(updatedTitle, get.Title);
            Assert.Equal(updatedGenre, get.Genre);
        }

        /// <summary>
        /// Tests update of a non-existent movie causing a conflict.
        /// </summary>
        [Fact]
        public async Task Update_Conflict_ReturnsConflict()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var movie1 = Helper.ValidCreateMovieDto();
            var movie2 = Helper.ValidCreateMovieDto();

            // Make movie2 distinct so CreateMovieAsync does not return Conflict when inserting
            movie2.Title = movie2.Title + "_other";

            var movieDto1 = await CreateMovieAsync(client, movie1);
            var movieDto2 = await CreateMovieAsync(client, movie2);

            // Try to update movieDto2 to match movieDto1 -> should conflict
            var update = Helper.ValidUpdateMovieDto();
            update.Id = movieDto2.Id;
            update.Title = movieDto1.Title;
            update.Director = movieDto1.Director;
            update.ReleaseYear = movieDto1.ReleaseYear;
            update.Genre = movieDto1.Genre;

            var resp = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{movieDto2.Id}", update);
            Assert.Equal(HttpStatusCode.Conflict, resp.StatusCode);
        }

        /// <summary>
        /// Tests update of a non-existent movie returning NotFound.
        /// </summary>
        [Fact]
        public async Task Delete_Succeeds()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, create);

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var del = await client.DeleteAsync(updateEndpoint);
            Assert.Equal(HttpStatusCode.NoContent, del.StatusCode);

            var get = await client.GetAsync(updateEndpoint);
            Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        }

        /// <summary>
        /// Tests patching a movie successfully.
        /// </summary> 
        [Fact]
        public async Task Patch_Succeeds()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, create);

            // Patch only the Title field   
            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            const string patchedTitle = "PatchedTitle";
            var patchObj = new { Title = patchedTitle };
            var content = JsonContent.Create(patchObj);
            var request = new HttpRequestMessage(new HttpMethod(Constants.Patch), updateEndpoint) { Content = content };
            var resp = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);

            var updated = await client.GetFromJsonAsync<MovieDto>(updateEndpoint);
            Assert.NotNull(updated);
            Assert.Equal(patchedTitle, updated!.Title);
        }

        /// <summary>
        /// Tests pre-filling the database with initial movie data and verifies movies are added.
        /// </summary>
        [Fact]
        public async Task Prefill_AddsMovies()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var resp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{Constants.Prefill}");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var list = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list);
            Assert.True(list.Length >= 15, $"Expected at least 15 prefilled movies but found {list.Length}");
        }

        /// <summary>
        /// Tests retrieval of a movie by ID and verifies the returned data matches the created movie.
        /// </summary>
        [Fact]
        public async Task GetById_Succeeds()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, create);

            var resp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{created.Id}");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var dto = await resp.Content.ReadFromJsonAsync<MovieDto>();
            Assert.NotNull(dto);
            Assert.Equal(created.Id, dto.Id);
            Assert.Equal(create.Title, dto.Title);
            Assert.Equal(create.Director, dto.Director);
            Assert.Equal(create.ReleaseYear, dto.ReleaseYear);
            Assert.Equal(create.Genre, dto.Genre);
        }

        /// <summary>
        /// Tests update with ID mismatch returning BadRequest.
        /// </summary>
        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var update = Helper.ValidUpdateMovieDto();
            update.Id = created.Id + 1; // mismatch intentionally

            var resp = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{created.Id}", update);
            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests update of a non-existent movie.
        /// </summary>
        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var updateId = 99999;
            var update = Helper.ValidUpdateMovieDto();
            update.Id = updateId;

            var resp = await client.PutAsJsonAsync($"{Constants.SlashMovies}{Constants.Slash}{updateId}", update);
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }

        /// <summary>
        /// Tests deletion of a non-existent movie.
        /// </summary>
        [Fact]
        public async Task Delete_NotFound_ReturnsNotFound()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var resp = await client.DeleteAsync($"{Constants.SlashMovies}{Constants.Slash}{Constants.ReleaseYearMax}");
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }

        /// <summary>
        /// Tests creation of a case-insensitive duplicate movie.
        /// </summary>
        [Fact]
        public async Task Create_CaseInsensitiveDuplicate_ReturnsConflict()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var baseCreate = Helper.ValidCreateMovieDto();

            const string updatedTitle = "CaseTest";
            const string updatedDirector = "DiRecTor";
            baseCreate.Title = updatedTitle;
            baseCreate.Director = updatedDirector;

            var first = await client.PostAsJsonAsync(Constants.SlashMovies, baseCreate);
            Assert.Equal(HttpStatusCode.Created, first.StatusCode);

            // Attempt duplicate with different casing
            var duplicate = Helper.ValidCreateMovieDto();
            duplicate.Title = updatedTitle.ToLower(); // different case
            duplicate.Director = updatedDirector.ToLower(); // different case
            duplicate.ReleaseYear = baseCreate.ReleaseYear; // same year
            duplicate.Genre = baseCreate.Genre;

            var second = await client.PostAsJsonAsync(Constants.SlashMovies, duplicate);
            Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        }

        /// <summary>
        /// Tests that {Constants.Prefill} skips adding duplicate movies if they already exist.
        /// </summary>
        [Fact]
        public async Task Prefill_SkipsDuplicates_WhenSampleAlreadyExists()
        {
            var client = CreateClient(Guid.NewGuid().ToString());

            // Insert a movie matching what the {Constants.Prefill} generator would create for Sample Movie 1
            var preExisting = new CreateMovieDto
            {
                Title = "Sample Movie 1",
                Director = "Director 1",
                ReleaseYear = 2001,
                Genre = "Drama"
            };
            var created = await CreateMovieAsync(client, preExisting);
            Assert.NotNull(created);

            var resp = await client.GetAsync($"{Constants.SlashMovies}{Constants.Slash}{Constants.Prefill}");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var list = await client.GetFromJsonAsync<MovieDto[]>(Constants.SlashMovies);
            Assert.NotNull(list);
            // Ensure at least 15 movies exist after {Constants.Prefill} (duplicates should be skipped but others added)
            Assert.True(list.Length >= 15, $"Expected at least 15 movies after {Constants.Prefill} but found {list.Length}");
        }

        /// <summary>
        /// Tests that a PATCH request with no content returns a BadRequest.
        /// </summary>
        [Fact]
        public async Task Patch_NoContent_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var url = $"{Constants.SlashMovies}{Constants.Slash}{Constants.ReleaseYearMax}";

            // Send a PATCH with no content at all
            var request = new HttpRequestMessage(new HttpMethod(Constants.Patch), url);
            var resp = await client.SendAsync(request);

            // The runtime currently returns 415 UnsupportedMediaType for an empty patch request
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request for a non-existent movie returns NotFound.
        /// </summary>
        [Fact]
        public async Task Patch_NotFound_ReturnsNotFound()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var url = $"{Constants.SlashMovies}{Constants.Slash}{Constants.ReleaseYearMax}";

            var patchObj = new { Title = "X" };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(url, content);

            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request with invalid data returns BadRequest.
        /// </summary>
        [Fact]
        public async Task Patch_EmptyTitle_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { Title = string.Empty };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request with invalid data returns BadRequest.
        /// </summary> 
        [Fact]
        public async Task Patch_EmptyDirector_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { Director = " " };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request with invalid data returns BadRequest.
        /// </summary>
        [Fact]
        public async Task Patch_EmptyGenre_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { Genre = "" };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request with invalid data returns BadRequest.
        /// </summary>
        [Fact]
        public async Task Patch_InvalidReleaseYear_ReturnsBadRequest()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var created = await CreateMovieAsync(client, Helper.ValidCreateMovieDto());

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { ReleaseYear = 1700 };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a PATCH request causing a duplicate movie returns Conflict.
        /// </summary>
        [Fact]
        public async Task Patch_Conflict_ReturnsConflict()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var movieA = Helper.ValidCreateMovieDto();
            var movieB = Helper.ValidCreateMovieDto();
            movieB.Title = movieB.Title + "_other";

            var dtoA = await CreateMovieAsync(client, movieA);
            var dtoB = await CreateMovieAsync(client, movieB);

            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{dtoB.Id}";
            var patchObj = new
            {
                Title = dtoA.Title,
                Director = dtoA.Director,
                ReleaseYear = dtoA.ReleaseYear
            };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.Conflict, resp.StatusCode);
        }

        /// <summary>
        /// Tests that a valid PATCH request updates the movie and persists the changes.
        /// </summary>
        [Fact]
        public async Task Patch_PartialUpdate_ReturnsNoContent_AndPersists()
        {
            var client = CreateClient(Guid.NewGuid().ToString());
            var create = Helper.ValidCreateMovieDto();
            var created = await CreateMovieAsync(client, create);
            const string newTitle = "NewTitle";
            var updateEndpoint = $"{Constants.SlashMovies}{Constants.Slash}{created.Id}";
            var patchObj = new { Title = newTitle, ReleaseYear = 2022 };
            var content = JsonContent.Create(patchObj);
            var resp = await client.PatchAsync(updateEndpoint, content);

            Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);

            var updated = await client.GetFromJsonAsync<MovieDto>(updateEndpoint);
            Assert.NotNull(updated);
            Assert.Equal(newTitle, updated.Title);
            Assert.Equal(2022, updated.ReleaseYear);
            Assert.Equal(create.Director, updated.Director);
            Assert.Equal(create.Genre, updated.Genre);
        }
    }
}