using Microsoft.EntityFrameworkCore;

namespace Cine.Models
{
    public class RoomsContext : DbContext
    {
        private readonly string _connectionString;

        public RoomsContext(DbContextOptions<RoomsContext> options)
            : base(options)
        {
            // Configura la cadena de conexión aquí
            _connectionString = "server=localhost; database=cine; user=root; password=";
        }

        public DbSet<Rooms> Rooms { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_connectionString);
            }
        }
    }
}
