using Microsoft.EntityFrameworkCore;

namespace Cine.Models
{
    public class RowContext : DbContext
    {
        public RowContext(DbContextOptions<RowContext> options)
            : base(options)
        {
        }

        public DbSet<Row> Rows { get; set; } = null!;
    }
}
