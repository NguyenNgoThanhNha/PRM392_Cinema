using PRM_API.Common.Payloads.Response;

public class SeatGroupResponse
{
    public string RowName { get; set; } = string.Empty;
    public List<SeatGroupDetailResponse> RowSeats { get; set; } = new();
}