namespace Cine.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public User user { get; set; }
        public List<Seat> seat { get; set; }


    }
}
