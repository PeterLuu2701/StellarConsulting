using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellar.Models;

namespace Stellar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/create-role
        [HttpPost("create-role")]
        [Authorize]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                return BadRequest("Role name is required.");
            }

            // Add the new role to the context
            _context.Roles.Add(role);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRole), 
                new { id = role.Id }, 
                role
            );
        }


        // GET: api/role
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // GET: api/role/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/role/update-role/{id}
        [HttpPut("update-role/{id}")]
        [Authorize]
        public async Task<ActionResult<Role>> UpdateRole(int id, Role role)
        {
            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole == null)
            {
                return NotFound($"Role with ID {id} not found.");
            }

            // Update only the fields that are allowed to change
            existingRole.Name = role.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while updating the role.");
            }

            return Ok(existingRole);
        }

        // DELETE: api/role/delete-role/{id}
        [HttpDelete("delete-role/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound($"Role with ID {id} not found.");
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok($"Role with ID {id} has been deleted.");
        }

    }
}
