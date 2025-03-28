using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellar.Models;
using Stellar.DTOs;

namespace Stellar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Retrieve the user from the database
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == model.Username);

            // Check if the user exists and if the password is correct
            if (user == null || user.Password != model.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

            var userInfo = new
            {
                user.Id,
                user.Name,
                user.RoleId,
                user.Role
            };

            // Return the user information along with a success message
            return Ok(new
            {
                Message = "Login successful.",
                User = userInfo
            });
        }
    }
}
