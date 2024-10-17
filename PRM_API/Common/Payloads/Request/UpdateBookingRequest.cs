using System.ComponentModel.DataAnnotations;
using PRM_API.Common.Enum;

namespace PRM_API.Common.Payloads.Request;

public class UpdateBookingRequest
{
    [Required(ErrorMessage = "BookingId is required!")]
    public int bookingId { get; set; }
    
    [Required(ErrorMessage = "StatusBooking is required!")]
    public StatusBooking status { get; set; }
}