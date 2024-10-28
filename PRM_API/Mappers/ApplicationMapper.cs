using AutoMapper;
using PRM_API.Dtos;
using PRM_API.Models;

namespace PRM_API.Mappers;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Movie, MovieDTO>().ReverseMap();
        CreateMap<Booking, BookingDTO>().ReverseMap();
        CreateMap<Seat, SeatDTO>().ReverseMap();
        CreateMap<BookingSeat, BookingSeatDTO>().ReverseMap();
        CreateMap<CinemaHall, HallDTO>().ReverseMap();
        CreateMap<Showtime, ShowtimeDTO>().ReverseMap();
    }
}