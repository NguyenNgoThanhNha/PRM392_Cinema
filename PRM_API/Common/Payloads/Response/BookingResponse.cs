using PRM_API.Dtos;
namespace PRM_API.Common.Payloads.Response;

public class BookingResponse
{
    public string? message { get; set; }
    public BookingDTO data { get; set; }
}