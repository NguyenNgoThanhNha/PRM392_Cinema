using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRM_API.Common.Payloads;
using PRM_API.Common.Payloads.Request;
using PRM_API.Dtos;
using PRM_API.Exceptions;
using PRM_API.Services;


namespace PRM_API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController:ControllerBase
    {
        private UserService _userService;
        private readonly IMapper _mapper;

        public UserController(UserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req) 
        {
            var result = _userService.Login(req.Email, req.Password);
            if (result is null) throw new BadRequestException("Wrong email or password");
            return Ok(ApiResult<UserDTO>.Succeed(_mapper.Map<UserDTO>(result)));
        }
    }
}
