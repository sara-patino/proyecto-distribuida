using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;

    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/Movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        try
        {
            var movies = await _context.Movies.ToListAsync();
            return Ok(movies);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Movies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        try
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    // POST: api/Movies
    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie(Movie movie)
    {
        try
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    // PUT: api/Movies/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }

        try
        {
            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    // DELETE: api/Movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        try
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}
