using Fan_platform.Data.Repositories;
using Fan_platform.Helpers;
using Fan_platform.Models;
using Fan_platform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fan_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;


        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the username or email is already in use
            var existingUser = await _userRepository.GetByUsernameOrEmailAsync(model.Username, model.Email);
            if (existingUser != null)
            {
                return Conflict("Username or email is already in use.");
            }

            // Create a new user entity
            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = _passwordHasher.HashPassword(null, model.Password) // Pass 'null' as user parameter
            };

            try
            {
                await _userRepository.AddAsync(newUser);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to register user.");
            }
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetByUsernameAsync(model.Username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid username or password.");
            }
            var secretKey = "secretkey123";
            var token = JwtTokenGenerator.GenerateToken(user,secretKey);

            return Ok(new { Token = token });
        }

        // GET: api/Users/profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileViewModel>> GetUserProfile()
        {
            var userId = User.Identity.Name;
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return new UserProfileViewModel
            {
                Username = user.Username,
                Email = user.Email
            };
        }

        // PUT: api/Users/profile
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile(UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.Name;
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Email = model.Email;

            try
            {
                await _userRepository.UpdateAsync(user);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update user profile.");
            }
        }
    }
}
