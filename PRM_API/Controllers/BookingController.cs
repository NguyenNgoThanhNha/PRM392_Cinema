using Microsoft.AspNetCore.Mvc;
using PRM_API.Common.Payloads;
using PRM_API.Common.Payloads.Request;
using PRM_API.Common.Payloads.Response;
using PRM_API.Services;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest createBookingRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResult<List<string>>.Error(errors));
            }

            var booking = await _bookingService.CreateBooking(createBookingRequest);
            if (booking.Equals(null))
            {
                return BadRequest(ApiResult<MessageResponse>.Error(new MessageResponse()
                {
                    message = "Error in create booking!"
                }));
            }

            return Ok(ApiResult<BookingResponse>.Succeed(new BookingResponse()
            {
                message = "Create new booking successfully!",
                data = booking
            }));
        }

        [HttpPut("update-status-booking")]
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingRequest updateBookingRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResult<List<string>>.Error(errors));
            }

            var booking = await _bookingService.UpdateStatusBooking(updateBookingRequest);
            if (booking.Equals(null))
            {
                return BadRequest(ApiResult<MessageResponse>.Error(new MessageResponse()
                {
                    message = "Error in update booking!"
                }));
            }

            return Ok(ApiResult<BookingResponse>.Succeed(new BookingResponse()
            {
                message = "Update booking successfully!",
                data = booking
            }));
        }
    }
}
