using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM_API.Common.Payloads;
using PRM_API.Common.Payloads.Request;
using PRM_API.Common.Payloads.Response;
using PRM_API.Services;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly SeatService _seatService;

        public SeatController(SeatService seatService)
        {
            _seatService = seatService;
        }
        [HttpGet("get-show-time-movie")]
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

            var showTimeMovie = await _seatService.GetShowTimeMovie(request.movieId);
            
            return Ok(ApiResult<GetShowTimeMovieResponse>.Succeed(new GetShowTimeMovieResponse()
            {
                data = showTimeMovie
            }));
        }
        
        [HttpGet("get-free-seat-movie")]
        public async Task<IActionResult> GetFreeSeatMovie([FromQuery] GetFreeSeatMovieRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResult<List<string>>.Error(errors));
            }

            var listSeat = await _seatService.GetFreeSeatMovie(request.showTimeId);
            if (listSeat.Equals(null) || !listSeat.Any())
            {
                return Ok(ApiResult<MessageResponse>.Succeed(new MessageResponse()
                {
                    message = $"List seat of movie with showtime {request.showTimeId} are null!"
                }));
            }
            
            return Ok(ApiResult<GetFreeSeatMovieResponse>.Succeed(new GetFreeSeatMovieResponse()
            {
                data = listSeat
            }));
        }
    }
}
