using AutoMapper;
using PRM_API.Dtos;
using PRM_API.Models;
using PRM_API.Repositories;

namespace PRM_API.Services
{
    public class UserService
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User,int> userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public UserDTO? Login(string email, string password) 
        {
            var result = _userRepository.FindByCondition(u => u.Email!.Equals(email) && u.Password.Equals(password))
                .FirstOrDefault();
            if (result is null) return null;
            return _mapper.Map<UserDTO>(result);
        }
    }
}
