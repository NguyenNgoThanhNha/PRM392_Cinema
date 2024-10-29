using System.Collections.Immutable;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRM_API.Common.Enum;
using PRM_API.Common.Payloads.Request;
using PRM_API.Common.Payloads.Response;
using PRM_API.Dtos;
using PRM_API.Exceptions;
using PRM_API.Models;
using PRM_API.Repositories;

namespace PRM_API.Services;

public class BookingService
{
    private readonly IRepository<Booking, int> _bookingRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<BookingSeat, int> _bookingSeatRepository;
    private readonly IRepository<BookingFoodBeverage, int> _fabRepository;

    public BookingService(IRepository<Booking, int> bookingRepository,
        IMapper mapper,
        IRepository<BookingSeat, int> bookingSeatRepository,
        IRepository<BookingFoodBeverage,int> fabRepository)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
        _bookingSeatRepository = bookingSeatRepository;
        _fabRepository = fabRepository;
    }

    public async Task<BookingDTO> CreateBooking(CreateBookingRequest request)
    {
        var bookingModel = new BookingDTO()
        {
            UserId = request.userId,
            ShowtimeId = request.showTimeId,
            BookingDate = DateTime.Now,
            TotalPrice = request.totalPrice,
            Status = StatusBooking.Processing.ToString()
        };
        var bookingEntity = _mapper.Map<Booking>(bookingModel);
        var bookingCreated = await _bookingRepository.AddAsync(bookingEntity);
        var result = await _bookingRepository.Commit();

        if (request.listSeatId != null && request.listSeatId.Any())
        {
            foreach (var seatId in request.listSeatId)
            {
                var bookingSeatDto = new BookingSeatDTO()
                {
                    BookingId = bookingCreated.BookingId,
                    SeatId = seatId,
                    BookingSeatStatus = StatusBooking.Processing.ToString()
                };

                var bookingSeatEntity = _mapper.Map<BookingSeat>(bookingSeatDto);
                await _bookingSeatRepository.AddAsync(bookingSeatEntity);
            }

            await _bookingSeatRepository.Commit();
        }

        if (result > 0)
        {
            return _mapper.Map<BookingDTO>(bookingCreated);
        }

        return null;
    }

    public async Task<BookingDTO> UpdateStatusBooking(UpdateBookingRequest request)
    {
        var bookingEntity = await _bookingRepository.GetByIdAsync(request.bookingId);

        if (bookingEntity == null)
        {
            throw new BadRequestException("Booking not found!");
        }

        if (!request.status.ToString().Equals(null))
        {
            bookingEntity.Status = request.status.ToString();
        }

        var updatedBooking = _bookingRepository.Update(bookingEntity);
        var result = await _bookingRepository.Commit();

        if (result > 0)
        {
            return _mapper.Map<BookingDTO>(updatedBooking);
        }
        return null;
    }

    public async Task<List<BookingSeatDTO>> GetBookedSeatsByShowTimeAsync(int showtimeId)
    {
        // Get booking order
        var booking = await _bookingRepository
            .FindByCondition(b => b.ShowtimeId == showtimeId)
            .FirstOrDefaultAsync(); 
        if(booking is null) return new();

        var query = _bookingSeatRepository.GetAll();

        return _mapper.Map<List<BookingSeatDTO>>(query.Where(bs => bs.BookingId == booking.BookingId).ToList());
    }

    public async Task<GetBookingDetailResponse?> GetBookingOrderDetails(int id)
    {
        var bookingSeats = await _bookingSeatRepository.FindByCondition(bs => bs.BookingId == id)
            .Include(bs => bs.Seat)
            .Select(bs => bs.Seat.SeatNumber)
            .ToListAsync();
        var fabs = _fabRepository.FindByCondition(fab => fab.BookingId == id)
            .Include(fab => fab.Food).AsQueryable();
        var fabsDetail = fabs.Select(fabs => new ShortFABDetails()
            {
                FoodName = fabs.Food.Name,
                Amount = fabs.Quantity
            })
            .ToList();

        var total = _bookingRepository.FindByCondition(bo => bo.BookingId == id).Select(b => b.TotalPrice).First();
        var fabTotal = fabs.Select(f => f.Quantity * f.Food.Price).Sum();
        
        
        var result = await _bookingRepository.FindByCondition(bo => bo.BookingId == id)
            .Include(b => b.User)
            .Include(b => b.Showtime)
            .ThenInclude(s => s.Hall)
            .Include(b => b.Showtime)
            .ThenInclude(s => s.Movie)
            .Select(b => new GetBookingDetailResponse()
            {
                BookingId = b.BookingId,
                UserName = b.User.Username,
                HallName = b.Showtime.Hall.HallName,
                MovieName = b.Showtime.Movie.Title,
                ShowDate = b.Showtime.ShowDate,
                BookingDate = b.BookingDate,
                SeatNames = bookingSeats,
                FABDetails = fabs.Any() ? fabsDetail: new List<ShortFABDetails>(),
                Status = b.Status,
                TotalPrice = total+fabTotal
            }).FirstOrDefaultAsync();
        return result;
    }
}