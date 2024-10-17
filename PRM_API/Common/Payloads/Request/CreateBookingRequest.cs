using System.ComponentModel.DataAnnotations;

namespace PRM_API.Common.Payloads.Request;

public class CreateBookingRequest
{
    [Required(ErrorMessage = "UserId is required!")]
    public int userId { get; set; }
    
    [Required(ErrorMessage = "ShowtimeId is required!")]
    public int showTimeId { get; set; }
    
    [Required(ErrorMessage = "SeatId is required!")]
    public List<int> listSeatId { get; set; }
    
    [Required(ErrorMessage = "TotalPrice is required!")]
    public decimal totalPrice { get; set; }
}