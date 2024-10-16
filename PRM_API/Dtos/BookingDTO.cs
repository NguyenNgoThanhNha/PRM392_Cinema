using PRM_API.Models;

namespace PRM_API.Dtos;

public class BookingDTO
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int ShowtimeId { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<BookingFoodBeverageDTO> BookingFoodBeverages { get; set; } = new List<BookingFoodBeverageDTO>();

    public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    public virtual Showtime Showtime { get; set; } = null!;
}