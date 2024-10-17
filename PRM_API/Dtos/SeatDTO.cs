namespace PRM_API.Dtos;

public class SeatDTO
{
    public int SeatId { get; set; }

    public int HallId { get; set; }

    public string SeatNumber { get; set; }

    public string SeatType { get; set; }

    /*    public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();*/

    public virtual HallDTO? Hall { get; set; }
}