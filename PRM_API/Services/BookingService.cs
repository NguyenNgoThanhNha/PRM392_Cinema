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
    private readonly IRepository<Seat, int> _seatRepo;

    public BookingService(IRepository<Booking, int> bookingRepository,
        IMapper mapper,
        IRepository<BookingSeat, int> bookingSeatRepository,
        IRepository<BookingFoodBeverage, int> fabRepository,
        IRepository<Seat, int> seatRepo)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
        _bookingSeatRepository = bookingSeatRepository;
        _fabRepository = fabRepository;
        _seatRepo = seatRepo;
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
            // Get selected seats
            var seats = await _seatRepo.GetAll().Where(s =>
                request.listSeatId!.Contains(s.SeatId)).ToListAsync();
            // Progress update status
            seats.ForEach(s => s.IsSold = true);
            // Save changes
            await _seatRepo.Commit();

            // Clear all booking in booking seat
            bookingCreated.BookingSeats.Select(bs => bs.Booking = null);

            // Map resp to DTO
            return _mapper.Map<BookingDTO>(bookingCreated);
        }

        return null!;
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
        if (booking is null) return new();

        var query = _bookingSeatRepository.GetAll();

        return _mapper.Map<List<BookingSeatDTO>>(query.Where(bs => bs.BookingId == booking.BookingId).ToList());
    }

    public async Task<GetBookingDetailResponse?> GetBookingOrderDetails(int id)
    {
        // Lấy danh sách ghế đã đặt
        var bookingSeats = await _bookingSeatRepository.FindByCondition(bs => bs.BookingId == id)
            .Include(bs => bs.Seat)
            .ToListAsync();

        // Lấy danh sách thức ăn/đồ uống đã đặt
        var fabs = await _fabRepository.FindByCondition(fab => fab.BookingId == id)
            .Include(fab => fab.Food)
            .ToListAsync();

        var fabsDetail = fabs.Select(fab => new ShortFABDetails
        {
            FoodName = fab.Food.Name,
            Amount = fab.Quantity,
            Price = fab.Food.Price,
        }).ToList();

        // Lấy tổng giá trị các mục Booking và FAB
        var bookingTotal = await _bookingRepository.FindByCondition(bo => bo.BookingId == id)
            .Select(b => b.TotalPrice)
            .FirstOrDefaultAsync();

        var fabTotal = fabs.Sum(f => f.Quantity * f.Food.Price);

        var seatPrice = await _bookingRepository.FindByCondition(bo => bo.BookingId == id)
        .Include(b => b.Showtime)
        .ThenInclude(s => s.Movie).FirstOrDefaultAsync();



        // Lấy chi tiết Booking
        var result = await _bookingRepository.FindByCondition(bo => bo.BookingId == id)
            .Include(b => b.User)
            .Include(b => b.Showtime)
                .ThenInclude(s => s.Hall)
            .Include(b => b.Showtime)
                .ThenInclude(s => s.Movie)
            .Select(b => new GetBookingDetailResponse
            {
                BookingId = b.BookingId,
                UserName = b.User.Username,
                HallName = b.Showtime.Hall.HallName,
                MovieName = b.Showtime.Movie.Title,
                ShowDate = b.Showtime.ShowDate,
                BookingDate = b.BookingDate,
                SeatNames = bookingSeats.Select(bs => new ShortSeatDetails
                {
                    HallId = bs.Seat.HallId,
                    SeatNumber = bs.Seat.SeatNumber,
                    SeatType = bs.Seat.SeatType,
                    SeatPrice = seatPrice.Showtime.SeatPrice > 0 ? seatPrice.Showtime.SeatPrice : 0
                }).ToList(),
                FABDetails = fabsDetail,
                Status = b.Status,
                TotalPrice = b.TotalPrice,
            }).FirstOrDefaultAsync();

        return result;
    }

    public async Task<List<GetListBookingOfUserResponse>?> GetListBookingOfUser(int id)
    {
        var listBookingUser = await _bookingRepository.FindByCondition(x => x.UserId == id)
            .Include(x => x.BookingSeats).ThenInclude(x => x.Seat)
            .Include(x => x.BookingFoodBeverages).ThenInclude(x => x.Food)
            .Include(x => x.Showtime)
            .Include(x => x.User)
            .ToListAsync();

        if (!listBookingUser.Any())
        {
            throw new BadRequestException("List booking of user not found!");
        }

        var response = listBookingUser.Select(item => new GetListBookingOfUserResponse
        {
            BookingId = item.BookingId,
            UserId = item.UserId,
            ShowtimeId = item.ShowtimeId,
            BookingDate = item.BookingDate,
            TotalPrice = item.TotalPrice,
            Status = item.Status,
            BookingFoodBeverages = item.BookingFoodBeverages.Select(fab => new ShortFABDetails
            {
                FoodName = fab.Food.Name,
                Amount = fab.Quantity,
                Price = fab.Food.Price,

            }).ToList(),
            BookingSeats = item.BookingSeats.Select(seat => new ShortSeatDetails
            {
                HallId = seat.Seat.HallId,
                SeatNumber = seat.Seat.SeatNumber,
                SeatType = seat.Seat.SeatType,
                SeatPrice = item.Showtime.SeatPrice > 0 ? item.Showtime.SeatPrice : 0
            }).ToList(),
            Showtime = _mapper.Map<ShowtimeDTO>(item.Showtime),
            User = _mapper.Map<UserDTO>(item.User)
        }).ToList();

        return response;
    }
}