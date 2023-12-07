using Microsoft.EntityFrameworkCore;

namespace Cine.Models
{
    public class SeatContext : DbContext
    {
        public SeatContext(DbContextOptions<SeatContext> options)
            : base(options)
        {
        }

        public DbSet<Seat> SeatItems { get; set; } = null!;
    }
}
