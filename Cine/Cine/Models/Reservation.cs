using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cine.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign key for Movie
        public int MovieId { get; set; }

        // Foreign key for Room
        public int RoomId { get; set; }

        // Navigation property for Seats
        public List<Seat> Seats { get; set; } = new List<Seat>(); // Inicializa la lista en el constructor

        public Reservation()
        {
            // Inicializa la propiedad Seats en el constructor
            Seats = new List<Seat>();
        }
    }
}
