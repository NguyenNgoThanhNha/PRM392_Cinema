namespace PRM_API.Dtos;

public class BookingDTO
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int ShowtimeId { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; }

    public List<BookingFoodBeverageDTO>? BookingFoodBeverages { get; set; }

    public List<BookingSeatDTO>? BookingSeats { get; set; } = new List<BookingSeatDTO>();

    public ShowtimeDTO? Showtime { get; set; }

    public UserDTO? User { get; set; }
}