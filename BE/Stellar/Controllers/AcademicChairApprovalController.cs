using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellar.Models;

namespace Stellar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicChairApprovalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AcademicChairApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get-all-course-outline
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CourseOutline>>> GetCourseOutlinesForAcademicChair()
        {
            // Only course outlines with approval status "Approved" - "Pending" will be displayed
            return await _context.CourseOutlines
                .Where(co => co.ProgramHeadApproval == "Approved" && co.AcademicChairApproval == "Pending")
                .Include(co => co.Instructor)
                .Include(co => co.PreparedByUser)
                .Include(co => co.ProgramCourse)
                    .ThenInclude(pc => pc.Program)
                .Include(co => co.ProgramCourse)
                    .ThenInclude(pc => pc.Course)
                .ToListAsync();
        }

        //academic-chair-approve
        [HttpPut("approve/{id}")]
        [Authorize]
        public async Task<IActionResult> AcademicChairApproveCourseOutline(int id)
        {
            var courseOutline = await _context.CourseOutlines.FindAsync(id);

            if (courseOutline == null)
            {
                return NotFound(); // Course outline with this ID doesn't exist
            }

            if (courseOutline.AcademicChairApproval == "Approved")
            {
                return Ok("This course outline has already been approved.");

            }
            else
            {
                // Set AcademicChairApproval to "Approved"
                courseOutline.AcademicChairApproval = "Approved";
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(courseOutline); // Successfully updated
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

        //academic-chair-reject
        [HttpPut("reject/{id}")]
        [Authorize]
        public async Task<IActionResult> AcademicChairRejectCourseOutline(int id)
        {
            var courseOutline = await _context.CourseOutlines.FindAsync(id);

            if (courseOutline == null)
            {
                return NotFound(); // Course outline with this ID doesn't exist
            }

            if (courseOutline.AcademicChairApproval == "Rejected")
            {
                return Ok("This course outline has already been rejected.");

            }
            else
            {
                // Set AcademicChairApproval to "Rejected"
                courseOutline.AcademicChairApproval = "Rejected";
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(courseOutline); // Successfully updated
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
