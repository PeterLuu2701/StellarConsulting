using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellar.Models;
using Stellar.DTOs;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace Stellar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == model.Username);

            if (user == null || user.Password != model.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"), // Keep the Role name if you have it loaded
                new Claim("userId", user.Id.ToString()),        // Add user ID as a string claim
                new Claim("username", user.Name),             // Add username as a string claim
                new Claim("roleId", user.RoleId.ToString())      // Add role ID as a string claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var userInfo = new
            {
                user.Id,
                user.Name,
                user.RoleId,
                Role = user.Role?.Name // Keep this in the response for immediate use
            };

            return Ok(new
            {
                Message = "Login successful.",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = userInfo
            });
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] dynamic model)
        {
            if (model == null ||
                string.IsNullOrEmpty((string)(model.name ?? "")) ||
                string.IsNullOrEmpty((string)(model.password ?? "")))
            {
                return BadRequest("Name and password are required.");
            }

            string username = model.name;
            string password = model.password;

            // 1. Check if username already exists (case-insensitive)
            var existingUser = await _context.Users
                .AnyAsync(u => u.Name!.ToLower() == username!.ToLower());

            if (existingUser)
            {
                return BadRequest("Username already exists.");
            }

            // Validate password with Regex
            // - At least 8 characters
            // - At least one uppercase letter
            // - At least one digit
            // - At least one special character
            var passwordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                return BadRequest("Password must be at least 8 characters long, contain at least one uppercase letter, one number, and one special character.");
            }

            // Set default role as "Instructor"
            var instructorRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Instructor");
            if (instructorRole == null)
            {
                return BadRequest("Instructor role not found.");
            }

            // Create new user
            var user = new User
            {
                Name = username,
                Password = password,  // Ideally, hash the password before storing
                RoleId = instructorRole.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userInfo = new
            {
                user.Id,
                user.Name,
                user.RoleId,
                Role = user.Role?.Name // Keep this in the response for immediate use
            };

            return CreatedAtAction(nameof(Login), new { username = user.Name }, new
            {
                Message = "User registered successfully.",
                User = userInfo
            });
        }


        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { Message = "Logout successful." });
        }
    }
}