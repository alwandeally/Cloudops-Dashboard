using CloudOpsDashboard.Api.Models;
using CloudOpsDashboard.Infrastructure.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudOpsDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CloudOpsDashboard.Api.Models.LoginRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Username and password are required."
                });
            }

            var username = request.Username.Trim();
            var password = request.Password.Trim();

            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u =>
                    u.Username.ToLower() == username.ToLower() &&
                    u.Password == password);

            if (user == null)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password."
                });
            }

            return Ok(new LoginResponse
            {
                Success = true,
                Username = user.Username,
                Role = user.Role,
                Message = "Login successful."
            });
        }
    }
}
