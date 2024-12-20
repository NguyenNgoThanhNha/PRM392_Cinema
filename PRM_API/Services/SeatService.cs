using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRM_API.Dtos;
using PRM_API.Exceptions;
using PRM_API.Models;
using PRM_API.Repositories;
using PRM_API.Services.Impl;
using System.Diagnostics.Contracts;

namespace PRM_API.Services;

public class SeatService
{
    private readonly IRepository<Seat, int> _seatRepository;
    
    private readonly IShowtimeService _showtimeService;
    private readonly IMapper _mapper;

    public SeatService(IRepository<Seat, int> seatRepository,
        IShowtimeService showtimeService,
        IMapper mapper)
    {
        _seatRepository = seatRepository;
        _mapper = mapper;
        _showtimeService = showtimeService;
    }

    public async Task<List<SeatDTO>> GetAllByShowTimeId(int showtimeId)
    {
        // Check exist showtime
        var showtimeDTO = await _showtimeService.GetAsync(showtimeId);
        if(showtimeDTO is null) return new();
    
        return _mapper.Map<List<SeatDTO>>(
            // Building query from IQueryable 
            _seatRepository.GetAll()
            // With conditions 
            .Where(s => s.HallId == showtimeDTO.HallId)
            // Convert to list
            .ToList());
    }
    
    public async Task<List<SeatDTO>> CheckSeatAvaiableAsync(List<int> seatIds)
    {
        var query = _seatRepository.GetAll();
        var seatEntities = await query.Where(s => seatIds.Contains(s.SeatId) && s.IsSold).ToListAsync();
        return _mapper.Map<List<SeatDTO>>(seatEntities);
    }

    public async Task<List<SeatDTO>> CheckExistIsOffSeatAsync(List<int> seatIds)
    {
        var query = _seatRepository.GetAll();
        var seatEntities = await query.Where(s => seatIds.Contains(s.SeatId) && s.IsOff).ToListAsync();
        return _mapper.Map<List<SeatDTO>>(seatEntities);
    }
}