using Microsoft.AspNetCore.Mvc;
using PRM_API.Common.Payloads;
using PRM_API.Common.Payloads.Request;
using PRM_API.Common.Payloads.Response;
using PRM_API.Exceptions;
using PRM_API.Services;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;
        private readonly SeatService _seatService;

        public BookingController(BookingService bookingService,
            SeatService seatService)
        {
            _bookingService = bookingService;
            _seatService = seatService;
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

            // Mark as valid seats selected
            var isValidSeatSelected = true;
            // Check whether seats are off 
            var offSeats = await _seatService.CheckExistIsOffSeatAsync(createBookingRequest.listSeatId);
            if (offSeats.Any())
            {
                // Mark as error invoke
                isValidSeatSelected = false;

                var existSeatNumbers = offSeats.Select(x => x.SeatNumber).ToArray();
                ModelState.AddModelError("listSeatId",
                    $"Some selected seats not exist, please check again.");
            }

            // Check whether seats are sold or not 
            var alreadySoldSeats = await _seatService.CheckSeatAvaiableAsync(createBookingRequest.listSeatId);
            if (alreadySoldSeats.Any())
            {
                // Mark as error invoke
                isValidSeatSelected = false;

                var existSeatNumbers = alreadySoldSeats.Select(x => x.SeatNumber).ToArray();
                ModelState.AddModelError("listSeatId",
                    $"Seat(s): {string.Join(", ", existSeatNumbers)} already sold.");
            }

            if (!isValidSeatSelected)
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingDetail([FromRoute] int id)
        {
            var result = await _bookingService.GetBookingOrderDetails(id);
            if (result is null) throw new BadRequestException("Something went wrong");
            return Ok(ApiResult<GetBookingDetailResponse>.Succeed(result));
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetListBookingOfUser([FromRoute] int userId)
        {
            try
            {
                var result = await _bookingService.GetListBookingOfUser(userId);
                return Ok(ApiResult<List<GetListBookingOfUserResponse>>.Succeed(result));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex));
            }
        }


    }
}
