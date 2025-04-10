using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellar.Models;

namespace Stellar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramHeadApprovalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProgramHeadApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get-all-course-outline
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CourseOutline>>> GetCourseOutlinesForProgramHead()
        {
            // Only course outlines with approval status "pending" - "pending" will be displayed
            return await _context.CourseOutlines
                .Where(co => co.ProgramHeadApproval == "Pending" && co.AcademicChairApproval == "Pending")
                .Include(co => co.Instructor)
                .Include(co => co.PreparedByUser)
                .Include(co => co.ProgramCourse)
                    .ThenInclude(pc => pc.Program)
                .Include(co => co.ProgramCourse)
                    .ThenInclude(pc => pc.Course)
                .ToListAsync();
        }

        //progam-head-approve
        [HttpPut("approve/{id}")]
        [Authorize]
        public async Task<IActionResult> ProgramHeadApproveCourseOutline(int id)
        {
            var courseOutline = await _context.CourseOutlines.FindAsync(id);

            if (courseOutline == null)
            {
                return NotFound(); // Course outline with this ID doesn't exist
            }

            if (courseOutline.ProgramHeadApproval == "Approved")
            {
                return Ok("This course outline has already been approved.");

            }
            else
            {
                // Set ProgramHeadApproval to "Approved"
                courseOutline.ProgramHeadApproval = "Approved";
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // Successfully updated
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseOutlineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return Conflict(); // Handle concurrency issues
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while approving the course outline.");
            }
        }

        //program-head-reject
        [HttpPut("reject/{id}")]
        [Authorize]
        public async Task<IActionResult> ProgramHeadRejectCourseOutline(int id)
        {
            var courseOutline = await _context.CourseOutlines.FindAsync(id);

            if (courseOutline == null)
            {
                return NotFound(); // Course outline with this ID doesn't exist
            }

            if (courseOutline.ProgramHeadApproval == "Rejected")
            {
                return Ok("This course outline has already been rejected.");

            }
            else
            {
                // Set ProgramHeadApproval to "Rejected"
                courseOutline.ProgramHeadApproval = "Rejected";
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // Successfully updated
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseOutlineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return Conflict(); // Handle concurrency issues
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while approving the course outline.");
            }
        }

        private bool CourseOutlineExists(int id)
        {
            return _context.CourseOutlines.Any(e => e.Id == id);
        }
    }
}
