using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cine.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        // Agrega una propiedad de navegación para la relación con Room
        public int RoomId { get; set; }
    }
}
