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
    public class RowsController : ControllerBase
    {
        private readonly RowContext _context;
        private readonly Connection _dbConnection;

        public RowsController(RowContext context)
        {
            _context = context;
            _dbConnection = new Connection();
        }

        // GET: api/Rows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Row>>> GetRows()
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
                            cmd.CommandText = "SELECT * FROM rows";

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                List<Row> rows = new List<Row>();

                                while (await reader.ReadAsync())
                                {
                                    Row row = new Row
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Name = reader["name"].ToString(),
                                        Seat = reader["seat"].ToString(),
                                        // Asegúrate de ajustar los tipos y nombres de columnas según tu esquema
                                    };

                                    rows.Add(row);
                                }

                                return rows;
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


        // GET: api/Rows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Row>> GetRows(int id)
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
                            cmd.CommandText = "SELECT * FROM rows WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    Row row = new Row
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Name = reader["name"].ToString(),
                                        Seat = reader["seat"].ToString(),
                                        // Asegúrate de ajustar los tipos y nombres de columnas según tu esquema
                                    };

                                    return row;
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


        // POST: api/Rows
        [HttpPost]
        public async Task<ActionResult<Row>> PostRow(Row row)
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
                            cmd.CommandText = "INSERT INTO rows (name, seat) VALUES (@name, @seat)";
                            cmd.Parameters.AddWithValue("@name", row.name);
                            cmd.Parameters.AddWithValue("@seat", row.seat);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                return CreatedAtAction(nameof(GetRows), new { id = row.Id }, row);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/Rows/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRow(int id, Row row)
        {
            if (id != row.Id)
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
                            cmd.CommandText = "UPDATE rows SET name = @name, seat = @seat WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@name", row.name);
                            cmd.Parameters.AddWithValue("@seat", row.seat);

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


        // DELETE: api/Rows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRow(int id)
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
                            cmd.CommandText = "SELECT * FROM rows WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    // Rows found, proceed with deletion
                                    reader.Close();

                                    cmd.CommandText = "DELETE FROM rows WHERE Id = @Id";
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


        private bool RowsExists(int id)
        {
            return _context.Rows.Any(e => e.Id == id);
        }
    }
}