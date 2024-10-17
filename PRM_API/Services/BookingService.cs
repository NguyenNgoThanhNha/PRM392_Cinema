using AutoMapper;
using PRM_API.Common.Enum;
using PRM_API.Common.Payloads.Request;
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

    public BookingService(IRepository<Booking, int> bookingRepository,
        IMapper mapper,
        IRepository<BookingSeat, int> bookingSeatRepository)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
        _bookingSeatRepository = bookingSeatRepository;
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

}