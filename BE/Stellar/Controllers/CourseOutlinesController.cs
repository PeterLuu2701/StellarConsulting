using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Stellar.DTOs;

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
            var courseOutlines = await _context.CourseOutlines
            .Include(co => co.Instructor)
            .Include(co => co.PreparedByUser)
            .Include(co => co.ApprovedByProgramHeadUser)
            .Include(co => co.ApprovedByAcademicChairUser)
            .Include(co => co.ProgramCourse)
                .ThenInclude(pc => pc.Program)
                    .ThenInclude(p => p.School)
                    .ThenInclude(s => s.Programs)
            .Include(co => co.ProgramCourse)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.ProgramCourses)
            .Include(co => co.LearningOutcomes) 
                .ThenInclude(lo => lo.LearningSteps) 
            .ToListAsync();

            return Ok(courseOutlines);
        }

        [HttpGet("{id}")]
        [Authorize] 
        public async Task<ActionResult<CourseOutline>> GetCourseOutline(int id)
        {
            var courseOutline = await _context.CourseOutlines
            .Include(co => co.Instructor)
            .Include(co => co.PreparedByUser)
            .Include(co => co.ApprovedByProgramHeadUser)
            .Include(co => co.ApprovedByAcademicChairUser)
            .Include(co => co.ProgramCourse)
                .ThenInclude(pc => pc.Program)
                    .ThenInclude(p => p.School)
                    .ThenInclude(s => s.Programs)
            .Include(co => co.ProgramCourse)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.ProgramCourses)
            .Include(co => co.LearningOutcomes)
                .ThenInclude(lo => lo.LearningSteps)
            .FirstOrDefaultAsync(co => co.Id == id);

            if (courseOutline == null)
            {
                return NotFound();
            }

            return Ok(courseOutline);
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

        //create-course-outline
        [HttpPost("create-course-outline")]
        [Authorize]
        public async Task<ActionResult<CourseOutline>> CreateCourseOutline(CourseOutlineDto dto)
        {
            var courseOutline = new CourseOutline
            {
                ProgramCourseId = dto.ProgramCourseId,
                AcademicYear = dto.AcademicYear,
                PreRequisites = dto.PreRequisites,
                CoRequisites = dto.CoRequisites,
                StudentAssessment = dto.StudentAssessment,
                PassingGrade = dto.PassingGrade,
                PlarMethod = dto.PlarMethod,
                InstructorId = dto.InstructorId,
                PreparedByUserId = dto.PreparedByUserId,
                PreparedDate = DateOnly.FromDateTime(DateTime.Now),
                ApprovedByProgramHeadUserId = dto.ApprovedByProgramHeadUserId,
                ApprovedByAcademicChairUserId = dto.ApprovedByAcademicChairUserId,
                ProgramHeadApproval = "Pending",
                AcademicChairApproval = "Pending"
            };

            _context.CourseOutlines.Add(courseOutline);
            await _context.SaveChangesAsync(); 

            foreach (var outcomeDto in dto.LearningOutcomes)
            {
                var outcome = new LearningOutcome
                {
                    OutcomeText = outcomeDto.OutcomeText,
                    LearningActivities = outcomeDto.LearningActivities,
                    CourseOutlineId = courseOutline.Id
                };

                _context.LearningOutcomes.Add(outcome);
                await _context.SaveChangesAsync(); 

                foreach (var stepDto in outcomeDto.LearningSteps)
                {
                    var step = new LearningStep
                    {
                        LearningOutcomeId = outcome.Id,
                        LearningText = stepDto.LearningText
                    };
                    _context.LearningSteps.Add(step);
                }
            }

            await _context.SaveChangesAsync(); 

            return CreatedAtAction(nameof(GetCourseOutline), new { id = courseOutline.Id }, courseOutline);
        }

        //copy
        [HttpPost("duplicate/{id}")]
        [Authorize]
        public async Task<ActionResult<CourseOutline>> DuplicateCourseOutline(int id)
        {
            var original = await _context.CourseOutlines
                .Include(co => co.LearningOutcomes)
                    .ThenInclude(lo => lo.LearningSteps)
                .FirstOrDefaultAsync(co => co.Id == id);

            if (original == null)
            {
                return NotFound();
            }

            var duplicate = new CourseOutline
            {
                ProgramCourseId = original.ProgramCourseId,
                AcademicYear = original.AcademicYear,
                PreRequisites = original.PreRequisites,
                CoRequisites = original.CoRequisites,
                StudentAssessment = original.StudentAssessment,
                PassingGrade = original.PassingGrade,
                PlarMethod = original.PlarMethod,
                InstructorId = original.InstructorId,
                PreparedByUserId = original.PreparedByUserId,
                PreparedDate = DateOnly.FromDateTime(DateTime.Now),
                ApprovedByProgramHeadUserId = null,
                ApprovedByAcademicChairUserId = null,
                ApprovedByProgramHeadDate = null,
                ApprovedByAcademicChairDate = null,
                ProgramHeadApproval = "Pending",
                AcademicChairApproval = "Pending"
            };

            _context.CourseOutlines.Add(duplicate);
            await _context.SaveChangesAsync(); 

            foreach (var outcome in original.LearningOutcomes)
            {
                var newOutcome = new LearningOutcome
                {
                    CourseOutlineId = duplicate.Id,
                    OutcomeText = outcome.OutcomeText,
                    LearningActivities = outcome.LearningActivities
                };

                _context.LearningOutcomes.Add(newOutcome);
                await _context.SaveChangesAsync(); 

                foreach (var step in outcome.LearningSteps)
                {
                    var newStep = new LearningStep
                    {
                        LearningOutcomeId = newOutcome.Id,
                        LearningText = step.LearningText
                    };

                    _context.LearningSteps.Add(newStep);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseOutline", new { id = duplicate.Id }, duplicate);
        }
    }
}