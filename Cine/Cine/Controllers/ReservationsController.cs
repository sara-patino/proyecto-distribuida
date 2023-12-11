using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cine.Models;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly ReservationContext _context;

    public ReservationsController(ReservationContext context)
    {
        _context = context;
    }

    // GET: api/Reservations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
    {
        try
        {
            var reservations = await _context.Reservations
                .Include(r => r.Seats) // Incluye los asientos relacionados
                .ToListAsync();

            return Ok(reservations);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Reservations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetReservation(int id)
    {
        try
        {
            var reservation = await _context.Reservations
                .Include(r => r.Seats) // Incluye los asientos relacionados
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }
    // POST: api/Reservations
    [HttpPost]
    public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
    {
        try
        {
            // Verifica si la sala existe
            var roomExists = await _context.Rooms.AnyAsync(r => r.Id == reservation.RoomId);
            if (!roomExists)
            {
                return BadRequest("La sala no existe.");
            }

            // Verifica si la película existe
            var movieExists = await _context.Movies.AnyAsync(m => m.Id == reservation.MovieId);
            if (!movieExists)
            {
                return BadRequest("La película no existe.");
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

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
            return BadRequest("ID mismatch");
        }

        try
        {
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

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
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    // POST: api/Reservations/CheckAvailability
    [HttpPost("CheckAvailability")]
    public async Task<ActionResult<bool>> CheckSeatAvailability([FromBody] Seat seat)
    {
        try
        {
            // Verifica si el asiento está disponible para la sala y película específicas
            var isSeatAvailable = await _context.Seats
                .Where(s => s.Row == seat.Row && s.Columnn == seat.Columnn)
                .AllAsync(s => s.ReservationId == null);

            return Ok(isSeatAvailable);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Reservations/SeatsForSingleMovie/{movieId}
    [HttpGet("SeatsForSingleMovie/{movieId}")]
    public async Task<ActionResult<IEnumerable<Seat>>> GetSeatsForSingleMovie(int movieId)
    {
        try
        {
            // Obtén todas las reservas que tengan la película específica y carga explícitamente las relaciones necesarias
            var reservations = await _context.Reservations
                .Include(r => r.Seats) // Asegúrate de incluir las relaciones de asientos
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

            // Obtén los asientos de esas reservas (incluso si no hay reservas)
            var seats = reservations
                .SelectMany(r => r.Seats)
                .ToList();

            // Retorna directamente los asientos, incluso si es un array vacío
            return Ok(seats);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    // DELETE: api/Reservations/DeleteAllForMovie/{movieId}
    [HttpDelete("DeleteAllForMovie/{movieId}")]
    public async Task<ActionResult<IEnumerable<Seat>>> DeleteAllForMovie(int movieId)
    {
        try
        {
            // Obtén todas las reservas asociadas a la película específica
            var reservationsToDelete = await _context.Reservations
                .Include(r => r.Seats)  // Incluye las seats asociadas a cada reserva
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

            if (reservationsToDelete == null || !reservationsToDelete.Any())
            {
                return NotFound($"No hay reservas para la película con ID {movieId}.");
            }

            // Elimina las seats asociadas a cada reserva
            foreach (var reservation in reservationsToDelete)
            {
                _context.Seats.RemoveRange(reservation.Seats);
            }

            // Elimina las reservas
            _context.Reservations.RemoveRange(reservationsToDelete);
            await _context.SaveChangesAsync();

            // Devuelve un array vacío
            return Ok(new List<Seat>());
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
