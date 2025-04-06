using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; 

namespace Stellar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseOutlinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseOutlinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CourseOutline>>> GetCourseOutlines()
        {
            return await _context.CourseOutlines.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize] 
        public async Task<ActionResult<CourseOutline>> GetCourseOutline(int id)
        {
            var courseOutline = await _context.CourseOutlines.FindAsync(id);

            if (courseOutline == null)
            {
                return NotFound();
            }

            return courseOutline;
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CourseOutline>>> SearchCourseOutlines(
            int? instructorId,
            int? courseId,
            int? programId,
            int? schoolId)
        {
            IQueryable<CourseOutline> query = _context.CourseOutlines
                .Include(co => co.ProgramCourse)
                    .ThenInclude(pc => pc.Course)
                .Include(co => co.ProgramCourse)
                    .ThenInclude(pc => pc.Program)
                        .ThenInclude(p => p.School);

            if (instructorId.HasValue)
            {
                query = query.Where(co => co.InstructorId == instructorId);
            }

            if (courseId.HasValue)
            {
                query = query.Where(co => co.ProgramCourse.CourseId == courseId);
            }

            if (programId.HasValue)
            {
                query = query.Where(co => co.ProgramCourse.ProgramId == programId);
            }

            if (schoolId.HasValue)
            {
                query = query.Where(co => co.ProgramCourse.Program.SchoolId == schoolId);
            }

            var courseOutlines = await query.ToListAsync();
            if (!courseOutlines.Any())
            {
                return NotFound("No course outlines found matching your criteria.");
            }
            return courseOutlines;
        }
    }
}