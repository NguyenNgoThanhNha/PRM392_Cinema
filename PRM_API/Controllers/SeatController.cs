using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using PRM_API.Common.Enum;
using PRM_API.Common.Payloads;
using PRM_API.Common.Payloads.Request;
using PRM_API.Common.Payloads.Response;
using PRM_API.Extensions;
using PRM_API.Services;
using PRM_API.Services.Impl;

namespace PRM_API.Controllers
{
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly SeatService _seatService;
        private readonly BookingService _bookingService;
        private readonly IShowtimeService _showtimeService;

        public SeatController(SeatService seatService,
            BookingService bookingService,
            IShowtimeService showtimeService)
        {
            _seatService = seatService;
            _bookingService = bookingService;
            _showtimeService = showtimeService;
        }

        [HttpGet(ApiRoute.ShowTime.GetAll)]
        public async Task<IActionResult> GetShowTimeMovie([FromQuery] GetShowTimeMovieRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResult<List<string>>.Error(errors));
            }

            var showTimeMovie = await _showtimeService.GetShowTimeMovieAsync(request.MovieId);
            
            return Ok(ApiResult<GetShowTimeMovieResponse>.Succeed(new GetShowTimeMovieResponse()
            {
                data = showTimeMovie
            }));
        }
        
        // [HttpGet("get-free-seat-movie")]
        // public async Task<IActionResult> GetFreeSeatMovie([FromQuery] GetFreeSeatMovieRequest request)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         var errors = ModelState.Values
        //             .SelectMany(v => v.Errors)
        //             .Select(e => e.ErrorMessage)
        //             .ToList();

        //         return BadRequest(ApiResult<List<string>>.Error(errors));
        //     }

        //     var listSeat = await _showtimeService.GetFreeSeatMovieAsync(request.showTimeId);
        //     if (listSeat.Equals(null) || !listSeat.Any())
        //     {
        //         return Ok(ApiResult<MessageResponse>.Succeed(new MessageResponse()
        //         {
        //             message = $"List seat of movie with showtime {request.showTimeId} are null!"
        //         }));
        //     }
            
        //     return Ok(ApiResult<GetFreeSeatMovieResponse>.Succeed(new GetFreeSeatMovieResponse()
        //     {
        //         data = listSeat
        //     }));
        // }
    
        [HttpGet(ApiRoute.Seat.GetAll)]
        public async Task<IActionResult> GetAllSeatByShowTimeAsync([FromQuery] int showtimeId)
        {
            var showtime = await _showtimeService.GetAsync(showtimeId);
            if(showtime is null) 
            {
                // return NotFound(ApiResult<MessageResponse>.Error(new MessageResponse()
                // {
                //     message = $"Not found any show time match {showtimeId}"
                // }));

                return Ok(new List<SeatGroupResponse>());
            }

            var seats = await _seatService.GetAllByShowTimeId(showtimeId);
            var bookedSeatIds = 
                (await _bookingService.GetBookedSeatsByShowTimeAsync(showtimeId))
                .Select(bs => bs.SeatId)
                .ToList();

            if (seats == null || !seats.Any())
            {
                // return NotFound(ApiResult<MessageResponse>.Error(new MessageResponse()
                // {
                //     message = $"List seat of movie with showtime {showtimeId} are null!"
                // }));

                return Ok(new List<SeatGroupResponse>());
            }

            var groupedSeats = seats.GroupBy(s => s.ColIndex).Select(rowGroup => new SeatGroupResponse
            {
                RowName = ((char)('A' + rowGroup.Key)).ToString(),
                RowSeats = rowGroup.Select(seat => new SeatGroupDetailResponse
                {
                    SeatId = seat.IsOff ? 0 : seat.SeatId,
                    SeatType = seat.IsOff ? "" : seat.SeatType,
                    Price = seat.IsOff 
                        ? 0
                        : seat.SeatType.Equals(SeatType.Normal.GetDescription())
                            ? showtime.SeatPrice
                            : ((decimal)(showtime.SeatPrice + 10000M)),
                    IsSeat = !seat.IsOff,
                    Name = seat.IsOff ? "" : seat.SeatNumber,
                    IsSold = seat.IsOff 
                        ? false 
                        : bookedSeatIds.Contains(seat.SeatId) ? true : false,
                    ColIndex = seat.ColIndex,
                    SeatIndex = seat.SeatIndex,
                }).ToList()
            }).ToList();

            return Ok(groupedSeats);
        }
    }
}
