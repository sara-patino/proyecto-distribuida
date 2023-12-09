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
    public class RervationsController : ControllerBase
    {
        private readonly ReservationContext _context;
        private readonly Connection _dbConnection;

        public ReservationsController(ReservationContext context)
        {
            _context = context;
            _dbConnection = new Connection();
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
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
                            cmd.CommandText = "SELECT * FROM reservations";

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                List<Reservation> reservations = new List<Reservation>();

                                while (await reader.ReadAsync())
                                {
                                    Reservation reservation = new Reservation
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Movie = reader["movie"].ToString(),
                                        User = reader["User"].ToString(),
                                        Seat = reader["seat"].ToString()
                                    };

                                    reservation.Add(reservation);
                                }

                                return reservation;
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


        // GET: api/Reservation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
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
                            cmd.CommandText = "SELECT * FROM reservations WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    Reservation reservation = new Reservation
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Movie = reader["movie"].ToString(),
                                        User = reader["User"].ToString(),
                                        Seat = reader["seat"].ToString()
                                    };

                                    return reservation;
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


        // POST: api/Reservation
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
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
                            cmd.CommandText = "INSERT INTO reservations (movie, user, seat) VALUES (@movie, @user, @seat)";
                            cmd.Parameters.AddWithValue("@movie", reservation.movie);
                            cmd.Parameters.AddWithValue("@user", reservation.user);
                            cmd.Parameters.AddWithValue("@seat", reservation.seat);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
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
                            cmd.CommandText = "UPDATE reservations SET movie = @movie, user = @user, seat = @seat WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@movie", reservation.movie);
                            cmd.Parameters.AddWithValue("@user", reservation.user);
                            cmd.Parameters.AddWithValue("@seat", reservation.seat);

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


        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
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
                            cmd.CommandText = "SELECT * FROM reservations WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", id);

                            using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    // reservation found, proceed with deletion
                                    reader.Close();

                                    cmd.CommandText = "DELETE FROM reservations WHERE Id = @Id";
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


        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}