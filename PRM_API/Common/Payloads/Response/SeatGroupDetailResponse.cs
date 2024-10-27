namespace PRM_API.Common.Payloads.Response;

public class SeatGroupDetailResponse
{
    public int SeatId { get; set; }
    public string SeatType { get; set; } = string.Empty;
    public decimal Price { get; set; } 
    public bool IsSeat { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSold { get; set; }
    public int ColIndex { get; set; }
    public int SeatIndex { get; set; }
}