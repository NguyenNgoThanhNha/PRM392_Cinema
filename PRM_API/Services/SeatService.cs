using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRM_API.Dtos;
using PRM_API.Exceptions;
using PRM_API.Models;
using PRM_API.Repositories;

namespace PRM_API.Services;

public class SeatService
{
    private readonly IRepository<Seat, int> _seatRepository;
    private readonly IRepository<Showtime, int> _showTimeRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<BookingSeat, int> _bookingSeatRepository;

    public SeatService(IRepository<Seat, int> seatRepository,
        IRepository<Showtime, int> showTimeRepository,
        IMapper mapper,
        IRepository<BookingSeat, int> bookingSeatRepository)
    {
        _seatRepository = seatRepository;
        _showTimeRepository = showTimeRepository;
        _mapper = mapper;
        _bookingSeatRepository = bookingSeatRepository;
    }

    public async Task<List<ShowtimeDTO>> GetShowTimeMovie(int movieId)
    {
        var showTimeMovie = await _showTimeRepository.FindByCondition(x => x.MovieId == movieId)
            .Include(x => x.Movie)
            .Include(x => x.Hall)
            .ToListAsync();
        if (showTimeMovie.Equals(null))
        {
            throw new BadRequestException("Showtime of movie not found!");
        }

        return _mapper.Map<List<ShowtimeDTO>>(showTimeMovie);
    }

    public async Task<List<SeatDTO>> GetFreeSeatMovie(int hallId)
    {
        var listSeat = await _seatRepository.FindByCondition(x => x.HallId == hallId).ToListAsync();
        if (listSeat.Equals(null) || !listSeat.Any())
        {
            return null;
        }

        var listSeatBooked = await _bookingSeatRepository.GetAll().ToListAsync();

        if (listSeatBooked.Any())
        {
            var freeSeats = listSeat
                .Where(seat => !listSeatBooked.Any(bookedSeat => bookedSeat.SeatId == seat.SeatId))
                .ToList();

            return _mapper.Map<List<SeatDTO>>(freeSeats);
        }
        return _mapper.Map<List<SeatDTO>>(listSeat);
    }
}