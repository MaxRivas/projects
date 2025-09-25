using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Dto;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    /// <summary>
    /// Controller that provides CRUD operations for <see cref="MoviesApi.Models.Movie"/> entities.
    /// </summary>
    public class MoviesController : ControllerBase
    {
        #region Private Fields
        private readonly MovieContext _db;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="MoviesController"/>.
        /// </summary>
        /// <param name="db">The <see cref="MovieContext"/> used for data access.</param>
        public MoviesController(MovieContext db) => _db = db;

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all movies.
        /// </summary>
        /// <returns>A collection of <see cref="MovieDto"/> representing all stored movies.</returns>
        [HttpGet()]
        public async Task<List<MovieDto>> GetAll()
        {
            return await _db.MovieItems
                .AsNoTracking()
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    ReleaseYear = m.ReleaseYear,
                    Genre = m.Genre
                })
                .ToListAsync();
        }

        /// <summary>
        /// Gets a single movie by id.
        /// </summary>
        /// <param name="id">The numeric id of the movie to retrieve.</param>
        /// <returns>An <see cref="ActionResult{MovieDto}"/> with the movie or <see cref="Microsoft.AspNetCore.Mvc.NotFoundResult"/>.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDto>> GetById(int id)
        {
            var movie = await _db.MovieItems.FindAsync(id);
            return null == movie
                ? (ActionResult<MovieDto>)NotFound()
                : (ActionResult<MovieDto>)new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Director = movie.Director,
                    ReleaseYear = movie.ReleaseYear,
                    Genre = movie.Genre
            };
        }

        /// <summary>
        /// Gets a list of movies to prefill the database.
        /// </summary>
        /// <returns>A list of <see cref="MovieDto"/> for pre-filling.</returns>
        [HttpGet("Prefill")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetPrefillMovies()
        {
            for(int i = 1; i <= 15; i++)
            {
                var prefill = new Movie
                {
                    Title = $"Sample Movie {i}",
                    Director = $"Director {i}",
                    ReleaseYear = 2000 + i,
                    Genre = "Drama"
                };
                
                // Skip adding if an identical movie already exists
                if (!await DuplicateExistsAsync(prefill.Title, prefill.Director, prefill.ReleaseYear))
                {
                    _db.MovieItems.Add(prefill);
                }
            }
            await _db.SaveChangesAsync();

            return Ok(new { Message = "Movies prefilled successfully." });
        }

        /// </summary>
        /// <param name="create">The DTO containing creation data.</param>
        /// <returns>Returns <see cref="Microsoft.AspNetCore.Mvc.CreatedAtActionResult"/> with the created <see cref="MovieDto"/>.</returns>
        [HttpPost]
        public async Task<ActionResult<MovieDto>> Create([FromBody] CreateMovieDto create)
        {
            if (ModelState.IsValid)
            {
                // Prevent duplicates: Title + Director + ReleaseYear
                if (await DuplicateExistsAsync(create.Title, create.Director, create.ReleaseYear))
                {
                    return Conflict(new { Message = "A movie with the same title, director and release year already exists." });
                }

                var movie = new Movie
                {
                    Title = create.Title,
                    Director = create.Director,
                    ReleaseYear = create.ReleaseYear,
                    Genre = create.Genre
                };
                _db.MovieItems.Add(movie);
                await _db.SaveChangesAsync();

                var dto = new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Director = movie.Director,
                    ReleaseYear = movie.ReleaseYear,
                    Genre = movie.Genre
                };
                return CreatedAtAction(nameof(GetById), new { id = movie.Id }, dto);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The id of the movie to update.</param>
        /// <param name="update">The update DTO containing new values.</param>
        /// <returns><see cref="Microsoft.AspNetCore.Mvc.IActionResult"/> representing the result of the operation.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieDto update)
        {
            if (ModelState.IsValid)
            {
                if (id == update.Id)
                {
                    var existing = await _db.MovieItems.FindAsync(id);
                    if (null != existing)
                    {
                        // Check for conflicts with other movies when changing identifying fields
                        if (await DuplicateExistsAsync(update.Title, update.Director, update.ReleaseYear, excludeId: id))
                        {
                            return Conflict(new { Message = "Another movie with the same title, director and release year already exists." });
                        }

                        existing.Title = update.Title;
                        existing.Director = update.Director;
                        existing.ReleaseYear = update.ReleaseYear;
                        existing.Genre = update.Genre;

                        await _db.SaveChangesAsync();
                        return NoContent();
                    }

                    return NotFound();
                }

                return BadRequest();
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a movie by id.
        /// </summary>
        /// <param name="id">The id of the movie to delete.</param>
        /// <returns><see cref="Microsoft.AspNetCore.Mvc.IActionResult"/> with NoContent on success or NotFound if missing.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _db.MovieItems.FindAsync(id);
            if (null != movie)
            {
                _db.MovieItems.Remove(movie);
                await _db.SaveChangesAsync();
                return NoContent();
            }

            return NotFound();
        }

        /// <summary>
        /// Partially updates a movie by applying non-null values from <see cref="PatchMovieDto"/>.
        /// </summary>
        /// <param name="id">The id of the movie to patch.</param>
        /// <param name="patch">The patch DTO with optional fields.</param>
        /// <returns>NoContent on success, NotFound if missing, or BadRequest for invalid input.</returns>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchMovieDto patch)
        {
            if (patch == null) return BadRequest();

            var existing = await _db.MovieItems.FindAsync(id);
            if (existing == null) return NotFound();

            // Apply fields only when present
            if (patch.Title != null) existing.Title = patch.Title;
            if (patch.Director != null) existing.Director = patch.Director;
            if (patch.ReleaseYear.HasValue) existing.ReleaseYear = patch.ReleaseYear.Value;
            if (patch.Genre != null) existing.Genre = patch.Genre;

            // Validate ReleaseYear range manually if provided
            if (patch.ReleaseYear.HasValue && (patch.ReleaseYear < 1888 || patch.ReleaseYear > 2100))
            {
                ModelState.AddModelError(nameof(patch.ReleaseYear), "ReleaseYear must be between 1888 and 2100.");
                return BadRequest(ModelState);
            }

            // After applying the patch, ensure we don't create a duplicate with another record
            if (await DuplicateExistsAsync(existing.Title, existing.Director, existing.ReleaseYear, excludeId: existing.Id))
            {
                return Conflict(new { Message = "Patching would create a duplicate movie (Title+Director+ReleaseYear)." });
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks for existing movie with same Title+Director+ReleaseYear. 
        /// </summary>
        /// <param name="title">title of movie</param>
        /// <param name="director">director of movie</param>
        /// <param name="releaseYear">release year of movie</param>
        /// <param name="excludeId">id to exclude from check (for updates)</param>
        /// <returns></returns>
        private Task<bool> DuplicateExistsAsync(string title, string director, int releaseYear, int? excludeId = null)
        {
            // Normalize for case-insensitive comparison. This is fine for the InMemory provider used in tests.
            var title = title?.ToLowerInvariant() ?? string.Empty;
            var director = director?.ToLowerInvariant() ?? string.Empty;

            var query = _db.MovieItems.AsQueryable();
            if (excludeId.HasValue) query = query.Where(movie => movie.Id != excludeId.Value);

            return query.AnyAsync(movie => movie.Title.ToLowerInvariant() == t &&
                                  movie.Director.ToLowerInvariant() == d &&
                                  movie.ReleaseYear == releaseYear);
        }

        #endregion
    }
}