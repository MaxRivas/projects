using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Models.Dto;

namespace MoviesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _db;

    public MoviesController(MovieContext db) => _db = db;

    [HttpGet]
    public async Task<IEnumerable<MovieDto>> GetAll()
    {
        return await _db.MovieItems
            .AsNoTracking()
            .Select(m => new MovieDto { Id = m.Id, Title = m.Title, Director = m.Director, ReleaseYear = m.ReleaseYear, Genre = m.Genre })
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDto>> GetById(int id)
    {
        var movie = await _db.MovieItems.FindAsync(id);
        if (movie == null) return NotFound();
        return new MovieDto { Id = movie.Id, Title = movie.Title, Director = movie.Director, ReleaseYear = movie.ReleaseYear, Genre = movie.Genre };
    }

    [HttpPost]
    public async Task<ActionResult<MovieDto>> Create([FromBody] CreateMovieDto create)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var movie = new Movie { Title = create.Title, Director = create.Director, ReleaseYear = create.ReleaseYear, Genre = create.Genre };
        _db.MovieItems.Add(movie);
        await _db.SaveChangesAsync();
        var dto = new MovieDto { Id = movie.Id, Title = movie.Title, Director = movie.Director, ReleaseYear = movie.ReleaseYear, Genre = movie.Genre };
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieDto update)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != update.Id) return BadRequest();
        var existing = await _db.MovieItems.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Title = update.Title;
        existing.Director = update.Director;
        existing.ReleaseYear = update.ReleaseYear;
        existing.Genre = update.Genre;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _db.MovieItems.FindAsync(id);
        if (movie == null) return NotFound();
        _db.MovieItems.Remove(movie);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
