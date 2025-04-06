using Microsoft.AspNetCore.Mvc;
using Stellar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Stellar.DTOs;

namespace Stellar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SchoolsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SchoolWithProgramsDto>>> GetAllSchoolsWithPrograms()
        {
            var schools = await _context.Schools
                .Include(s => s.Programs)
                .Select(s => new SchoolWithProgramsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Programs = s.Programs.Select(p => new ProgramInfoDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList()
                })
                .ToListAsync();

            if (!schools.Any())
            {
                return NotFound("No schools found.");
            }
            return schools;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<SchoolWithProgramsDto>> GetSchoolWithProgramsById(int id)
        {
            var school = await _context.Schools
                .Include(s => s.Programs)
                .Where(s => s.Id == id)
                .Select(s => new SchoolWithProgramsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Programs = s.Programs.Select(p => new ProgramInfoDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (school == null)
            {
                return NotFound($"School with ID {id} not found.");
            }
            return school;
        }
    }
}
