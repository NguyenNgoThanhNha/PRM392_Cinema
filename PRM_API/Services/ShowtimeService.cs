using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRM_API.Dtos;
using PRM_API.Exceptions;
using PRM_API.Models;
using PRM_API.Repositories;
using PRM_API.Services.Impl;

namespace PRM_API.Services;

public class ShowtimeService : IShowtimeService
{
    private IRepository<Showtime, int> _repo;
    private IRepository<Seat, int> _seatRepo;
    private IRepository<BookingSeat, int> _bookingSeatRepo;
    
    private readonly IMapper _mapper;

    public ShowtimeService(
        IRepository<Showtime, int> repo,
        IRepository<BookingSeat, int> bookingSeatRepo,
        IRepository<Seat, int> seatRepo,
        IMapper mapper)
    {
        _repo = repo;
        _seatRepo = seatRepo;
        _bookingSeatRepo = bookingSeatRepo;
        _mapper = mapper;        
    }


    public async Task<ShowtimeDTO> GetAsync(int showtimeId)
    {
        return _mapper.Map<ShowtimeDTO>(await _repo.GetByIdAsync(showtimeId));
    }

    public async Task<List<ShowtimeDTO>> GetShowTimeMovieAsync(int movieId)
    {
        var showTimeMovie = await _repo.FindByCondition(x => x.MovieId == movieId)
            .Include(x => x.Movie)
            .Include(x => x.Hall)
            .ToListAsync();
        if (showTimeMovie.Equals(null))
        {
            throw new BadRequestException("Showtime of movie not found!");
        }

        return _mapper.Map<List<ShowtimeDTO>>(showTimeMovie);
    }

    public async Task<List<SeatDTO>> GetFreeSeatMovieAsync(int showTimeId)
    {
        var showTime = await _repo.GetByIdAsync(showTimeId);
        var listSeat = await _seatRepo.FindByCondition(x => x.HallId == showTime!.HallId).ToListAsync();
        if (listSeat.Equals(null) || !listSeat.Any())
        {
            return null!;
        }

        var listSeatBooked = await _bookingSeatRepo.GetAll().ToListAsync();

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