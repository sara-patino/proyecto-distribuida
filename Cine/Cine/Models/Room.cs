namespace Cine.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int RoomName { get; set; }

        public List<Row> Rows { get; set; }

        public Boolean IsOccupied { get; set; }
    }
}
