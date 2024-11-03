namespace PRM_API.Common.Payloads.Response;

public class GetBookingDetailResponse
{
    public int BookingId { get; set; }

    public string UserName { get; set; }
    public string HallName { get; set; }
    public string MovieName { get; set; }
    public DateTime ShowDate { get; set; }

    public DateTime BookingDate { get; set; }

    public List<ShortSeatDetails> SeatNames { get; set; }
    public List<ShortFABDetails> FABDetails { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

}

public struct ShortFABDetails
{
    public string FoodName { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
}

