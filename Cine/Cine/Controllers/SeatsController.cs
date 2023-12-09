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
    public class SeatsController : ControllerBase
    {
        private readonly SeatContext _context;
        private readonly Connection _dbConnection;

        public SeatsController(SeatContext context)
        {
            _context = context;
            _dbConnection = new Connection();
        }

        // GET: api/Seats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seat>>> GetSeats()
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
                            cmd.CommandText = "SELECT * FROM seats";

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                List<Seat> seats = new List<Seat>();

                                while (await reader.ReadAsync())
                                {
                                    Seat seat = new Seat
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Number = Convert.ToInt32(reader[" Number"]),
                                        isEmpty = reader["isEmpty"].ToString(),
                                        // Asegúrate de ajustar los tipos y nombres de columnas según tu esquema
                                    };

                                    seats.Add(seat);
                                }

                                return seats;
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


        // GET: api/Seats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seat>> GetSeats(int id)
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
                            cmd.CommandText = "SELECT * FROM seats WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    Seat seat = new Seat
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Number = Convert.ToInt32(reader[" Number"]),
                                        isEmpty = reader["isEmpty"].ToString(),
                                        // Asegúrate de ajustar los tipos y nombres de columnas según tu esquema
                                    };

                                    return seat;
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


        // POST: api/Seat
        [HttpPost]
        public async Task<ActionResult<Seat>> PostSeat(Seat seat)
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
                            cmd.CommandText = "INSERT INTO seats (number, isempty) VALUES (@number, @isempty)";
                            cmd.Parameters.AddWithValue("@number", seat.number);
                            cmd.Parameters.AddWithValue("@isempty", seat.isempty);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                return CreatedAtAction(nameof(GetSeats), new { id = seat.Id }, seat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/Seats/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeat(int id, Seat seat)
        {
            if (id != seat.Id)
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
                            cmd.CommandText = "UPDATE seat SET number = @number, isempty = @isempty WHERE Id = @Id";
                            cmd.CommandText = "INSERT INTO seats (number, isempty) VALUES (@number, @isempty)";
                            cmd.Parameters.AddWithValue("@number", seat.number);
                            cmd.Parameters.AddWithValue("@isempty", seat.isempty);

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


        // DELETE: api/Seats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
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
                            cmd.CommandText = "SELECT * FROM seats WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    // Seat found, proceed with deletion
                                    reader.Close();

                                    cmd.CommandText = "DELETE FROM seats WHERE Id = @Id";
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


        private bool SeatExists(int id)
        {
            return _context.Seats.Any(e => e.Id == id);
        }
    }
}