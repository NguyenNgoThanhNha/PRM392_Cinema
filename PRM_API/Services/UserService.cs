using AutoMapper;
using PRM_API.Common.Payloads.Request;
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
        
        public async Task<UserDTO> SignUp(SignUpRequest request)
        {
            var userModel = new UserDTO()
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };
            var userEntity = _mapper.Map<User>(userModel);
            var userCreated = await _userRepository.AddAsync(userEntity);
            var result = await _userRepository.Commit();
            if (result > 0)
            {
                return _mapper.Map<UserDTO>(userCreated);
            }

            return null;
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
