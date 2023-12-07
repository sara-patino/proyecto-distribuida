using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cine.Models;
using MySql.Data.MySqlClient;

namespace Cine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly Connection _dbConnection;

        public MoviesController(MovieContext context)
        {
            _context = context;
            _dbConnection = new Connection();
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            try
            {
                using (MySqlConnection connection = _dbConnection.Connect())
                {
                    if (connection != null)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT * FROM movies";

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                List<Movie> movies = new List<Movie>();

                                while (await reader.ReadAsync())
                                {
                                    Movie movie = new Movie
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Title = reader["title"].ToString(),
                                        startTime = reader["starttime"].ToString(),
                                        endTime = reader["endtime"].ToString()
                                        // Asegúrate de ajustar los tipos y nombres de columnas según tu esquema
                                    };

                                    movies.Add(movie);
                                }

                                return movies;
                            }
                        }
                    }
                }
               return NotFound(); // O un valor predeterminado según tus necesidades
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
                using (MySqlConnection connection = _dbConnection.Connect())
                {
                    if (connection != null)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT * FROM movies WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    Movie movie = new Movie
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Title = reader["title"].ToString(),
                                        startTime = reader["starttime"].ToString(),
                                        endTime = reader["endtime"].ToString()
                                        // Asegúrate de ajustar los tipos y nombres de columnas según tu esquema
                                    };

                                    return movie;
                                }
                                else
                                {
                                    return NotFound();
                                }
                            }
                        }
                    }
                }

                // Agrega un return en caso de que no haya datos o haya un problema con la conexión
                return NotFound(); // O un valor predeterminado según tus necesidades
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
                using (MySqlConnection connection = _dbConnection.Connect())
                {
                    if (connection != null)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = "INSERT INTO movies (title, starttime, endtime) VALUES (@title, @starttime, @endtime)";
                            cmd.Parameters.AddWithValue("@title", movie.Title);
                            cmd.Parameters.AddWithValue("@starttime", movie.startTime);
                            cmd.Parameters.AddWithValue("@endtime", movie.endTime);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

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
                using (MySqlConnection connection = _dbConnection.Connect())
                {
                    if (connection != null)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = "UPDATE movies SET title = @title, starttime = @starttime, endtime = @endtime WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@title", movie.Title);
                            cmd.Parameters.AddWithValue("@starttime", movie.startTime);
                            cmd.Parameters.AddWithValue("@endtime", movie.endTime);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

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
                using (MySqlConnection connection = _dbConnection.Connect())
                {
                    if (connection != null)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT * FROM movies WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    // Movie found, proceed with deletion
                                    reader.Close();

                                    cmd.CommandText = "DELETE FROM movies WHERE Id = @Id";
                                    await cmd.ExecuteNonQueryAsync();

                                    return NoContent();
                                }
                                else
                                {
                                    return NotFound();
                                }
                            }
                        }
                    }
                }

                // Agrega un return en caso de que no haya datos o haya un problema con la conexión
                return NotFound(); // O un valor predeterminado según tus necesidades
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
}
