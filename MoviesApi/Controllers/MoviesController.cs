using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Dto;

namespace MoviesApi.Controllers;

[ApiController]
[Route("[controller]")]
/// <summary>
/// Controller that provides CRUD operations for <see cref="MoviesApi.Models.Movie"/> entities.
/// </summary>
public class MoviesController : ControllerBase
{
    private readonly MovieContext _db;

    /// <summary>
    /// Creates a new instance of <see cref="MoviesController"/>.
    /// </summary>
    /// <param name="db">The <see cref="MovieContext"/> used for data access.</param>
    public MoviesController(MovieContext db) => _db = db;

    /// <summary>
    /// Returns all movies.
    /// </summary>
    /// <returns>A collection of <see cref="MovieDto"/> representing all stored movies.</returns>
    [HttpGet]
    public async Task<IEnumerable<MovieDto>> GetAll()
    {
        return await _db.MovieItems
            .AsNoTracking()
            .Select(m => new MovieDto { Id = m.Id, Title = m.Title, Director = m.Director, ReleaseYear = m.ReleaseYear, Genre = m.Genre })
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
            ? (ActionResult<MovieDto>) NotFound()
            : (ActionResult<MovieDto>) new MovieDto { Id = movie.Id, Title = movie.Title, Director = movie.Director, ReleaseYear = movie.ReleaseYear, Genre = movie.Genre };
    }

    /// <summary>
    /// Creates a new movie from the provided <see cref="CreateMovieDto"/>.
    /// </summary>
    /// <param name="create">The DTO containing creation data.</param>
    /// <returns>Returns <see cref="Microsoft.AspNetCore.Mvc.CreatedAtActionResult"/> with the created <see cref="MovieDto"/>.</returns>
    [HttpPost]
    public async Task<ActionResult<MovieDto>> Create([FromBody] CreateMovieDto create)
    {
        if (ModelState.IsValid)
        {
            var movie = new Movie { Title = create.Title, Director = create.Director, ReleaseYear = create.ReleaseYear, Genre = create.Genre };
            _db.MovieItems.Add(movie);
            await _db.SaveChangesAsync();

            var dto = new MovieDto { Id = movie.Id, Title = movie.Title, Director = movie.Director, ReleaseYear = movie.ReleaseYear, Genre = movie.Genre };
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
}