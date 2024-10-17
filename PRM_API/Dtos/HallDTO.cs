namespace PRM_API.Dtos;

public class HallDTO
{
    public int HallId { get; set; }

    public string HallName { get; set; }

    public int TotalSeats { get; set; }

    /*    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();*/
}