using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellar.Models;
using Stellar.DTOs; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Stellar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProgramInfoDto>>> GetAllPrograms()
        {
            var programs = await _context.Programs
                .Include(p => p.School)
                .Include(p => p.Degree)
                .ToListAsync();

            if (!programs.Any())
            {
                return NotFound("No programs found.");
            }

            var programDtos = programs.Select(p => new ProgramInfoDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                SchoolId = p.SchoolId,
                DegreeId = p.DegreeId
            }).ToList();

            return Ok(programDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProgramInfoDto>> GetProgramById(int id)
        {
            var program = await _context.Programs
                .Include(p => p.School)
                .Include(p => p.Degree)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (program == null)
            {
                return NotFound($"Program with ID {id} not found.");
            }

            var programDto = new ProgramInfoDto
            {
                Id = program.Id,
                Name = program.Name,
                Description = program.Description,
                SchoolId = program.SchoolId,
                DegreeId = program.DegreeId
            };

            return Ok(programDto);
        }
    }
}