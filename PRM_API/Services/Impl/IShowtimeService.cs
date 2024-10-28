using PRM_API.Dtos;

namespace PRM_API.Services.Impl;

public interface IShowtimeService
{
    Task<ShowtimeDTO> GetAsync(int showtimeId);
    Task<List<ShowtimeDTO>> GetShowTimeMovieAsync(int movieId);
    Task<List<SeatDTO>> GetFreeSeatMovieAsync(int showTimeId);
}