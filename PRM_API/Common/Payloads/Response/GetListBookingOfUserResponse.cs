using PRM_API.Dtos;

namespace PRM_API.Common.Payloads.Response;

public class GetListBookingOfUserResponse
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int ShowtimeId { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public List<ShortFABDetails>? BookingFoodBeverages { get; set; }

    public List<ShortSeatDetails>? BookingSeats { get; set; }

    public ShowtimeDTO Showtime { get; set; } = null!;

    public UserDTO User { get; set; } = null!;
}

public struct ShortSeatDetails
{
    public ShortSeatDetails()
    {
    }

    public int HallId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public string? SeatType { get; set; } = null!;
    public decimal? SeatPrice { get; set; }
}