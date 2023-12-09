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
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly Connection _dbConnection;

        public UsersController(UserContext context)
        {
            _context = context;
            _dbConnection = new Connection();
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
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
                            cmd.CommandText = "SELECT * FROM users";

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                List<User> users = new List<User>();

                                while (await reader.ReadAsync())
                                {
                                    User user = new User
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        name = reader["name"].ToString(),
                                        email = reader["email"].ToString(),
                                        // Asegúrate de ajustar los tipos y nombres de columnas según tu esquema
                                    };

                                    users.Add(user);
                                }

                                return users;
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


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUsers(int id)
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
                            cmd.CommandText = "SELECT * FROM users WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    User user = new User
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        name = reader["name"].ToString(),
                                        email = reader["email"].ToString(),
                                        // Asegúrate de ajustar los tipos y nombres de columnas según tu esquema
                                    };

                                    return user;
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


        // POST: api/USer
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
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
                            cmd.CommandText = "INSERT INTO users (name, email) VALUES (@name, @email)";
                            cmd.Parameters.AddWithValue("@name", user.name);
                            cmd.Parameters.AddWithValue("@email", user.email);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, User user)
        {
            if (id != user.Id)
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
                            cmd.CommandText = "UPDATE user SET name = @name, email = @email WHERE Id = @Id";
                            cmd.CommandText = "INSERT INTO user (name, email) VALUES (@name, @email)";
                            cmd.Parameters.AddWithValue("@name", user.name);
                            cmd.Parameters.AddWithValue("@email", user.email);

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


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
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
                            cmd.CommandText = "SELECT * FROM users WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    // User found, proceed with deletion
                                    reader.Close();

                                    cmd.CommandText = "DELETE FROM users WHERE Id = @Id";
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


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}