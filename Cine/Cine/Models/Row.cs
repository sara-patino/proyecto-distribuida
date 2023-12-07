namespace Cine.Models
{
    public class Row
    {
        public int Id { get; set; }
        public char RowName { get; set; }

        public List<Seat> seats { get; set; }
    }
}
