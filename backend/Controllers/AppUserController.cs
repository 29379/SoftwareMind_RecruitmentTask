using AutoMapper;
using HotDeskBookingSystem.Data.Dto.User;
using HotDeskBookingSystem.Interfaces.Repositories;
using HotDeskBookingSystem.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotDeskBookingSystem.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public AppUserController(IAppUserRepository appUserRepository, IJwtTokenService jwtTokenService, IPasswordService passwordService, IMapper mapper)
        {
            _appUserRepository = appUserRepository;
            _jwtTokenService = jwtTokenService;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterDto registerDto)
        {
            if (await _appUserRepository.UserExistsAsync(registerDto.Email))
            {
                return BadRequest("User already exists");
            }

            var createdUser = await _appUserRepository.RegisterUserAsync(registerDto);
            if (createdUser == null)
            {
                return StatusCode(500, "Error registering new user");
            }

            var token = _jwtTokenService.GenerateJwtToken(createdUser);
            return Ok(new
            {
                User = _mapper.Map<AppUserDto>(createdUser),
                Token = token
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto loginDto)
        {

            var foundUser = await _appUserRepository
                .LoginUserAsync(loginDto);
            if (foundUser == null)
            {
                return Unauthorized("Bad credentials. User with email: " + loginDto.Email + " not found.");
            }

            var token = _jwtTokenService.GenerateJwtToken(foundUser);
            return Ok(new
            {
                User = _mapper.Map<AppUserDto>(foundUser),
                Token = token
            });
        }

        [HttpGet("all")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _appUserRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{email}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var user = await _appUserRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpGet("me")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetMeAsync()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (email == null)
            {
                return Unauthorized();
            }
            var user = await _appUserRepository
                .GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpPut]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto updatedUserDto)
        {
            var requestingUserEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            var requestingUserRoles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (requestingUserEmail != updatedUserDto.Email && !requestingUserRoles.Contains("ADMIN"))
            {
                return Unauthorized("You do not have permission to update this user");
            }

            var result = await _appUserRepository
                .UpdateUserAsync(updatedUserDto);
            if (result == null)
            {
                return StatusCode(500, "Error updating user");
            }

            var token = _jwtTokenService.GenerateJwtToken(result);
            return Ok(new
            {
                User = _mapper.Map<AppUserDto>(result),
                Token = token
            });
        }


        [HttpDelete("{email}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> DeleteUserAsync(string email)
        {
            var requestingUserEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            var requestingUserRoles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (requestingUserEmail != email && !requestingUserRoles.Contains("ADMIN"))
            {
                return Unauthorized("You do not have permission to delete this user");
            }

            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            var deletedUser = await _appUserRepository
                .DeleteUserAsync(email);
            if (deletedUser == null)
            {
                return NotFound("User " + email + " not found");
            }

            if (jti != null)
            {
                _jwtTokenService.RemoveToken(jti);
            }

            return Ok(deletedUser);
        }

    }
}
