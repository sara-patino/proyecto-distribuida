using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cine.Models
{
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Row { get; set; }

        public int Columnn { get; set; }

        // Foreign key for Reservation
        public int ReservationId { get; set; }

    }
}
