﻿using Microsoft.EntityFrameworkCore;

namespace Cine.Models
{
    public class MovieContext : DbContext
    {
        private readonly string _connectionString;

        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
            // Configura la cadena de conexión aquí
            _connectionString = "server=localhost; database=cine; user=root; password=";
        }

        public DbSet<Movies> Movies { get; set; } = null!;
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
