using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MoviesDbContext _db;
    public MoviesController(MoviesDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? q, [FromQuery] string? genre, [FromQuery] string? director)
    {
        var query = _db.Movies.AsQueryable();
        if (!string.IsNullOrEmpty(q)) query = query.Where(m => m.Title.Contains(q));
        if (!string.IsNullOrEmpty(genre)) query = query.Where(m => m.Genre == genre);
        if (!string.IsNullOrEmpty(director)) query = query.Where(m => m.Director != null && m.Director.Contains(director));
        var list = await query.ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var m = await _db.Movies.FindAsync(id);
        if (m == null) return NotFound(new { code = "NOT_FOUND", message = "Movie not found" });
        return Ok(m);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Movie movie)
    {
        movie.CreatedAt = DateTime.UtcNow;
        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Movie updated)
    {
        var m = await _db.Movies.FindAsync(id);
        if (m == null) return NotFound(new { code = "NOT_FOUND", message = "Movie not found" });
        m.Title = updated.Title;
        m.Genre = updated.Genre;
        m.Director = updated.Director;
        m.ReleaseDate = updated.ReleaseDate;
        await _db.SaveChangesAsync();
        return Ok(m);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var m = await _db.Movies.FindAsync(id);
        if (m == null) return NotFound(new { code = "NOT_FOUND", message = "Movie not found" });
        _db.Movies.Remove(m);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
