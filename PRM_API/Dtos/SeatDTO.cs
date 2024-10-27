namespace PRM_API.Dtos;

public class SeatDTO
{
    public int SeatId { get; set; }

    public int HallId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public string SeatType { get; set; } = null!;

    public bool IsOff { get; set; }

    public bool IsSold { get; set; }

    public int SeatIndex { get; set; }

    public int ColIndex { get; set; }

    // public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    public virtual HallDTO? Hall { get; set; } = null!;
}