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

    }
}