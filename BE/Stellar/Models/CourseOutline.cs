using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class CourseOutline
{
    public int Id { get; set; }

    public int? ProgramCourseId { get; set; }

    public string? AcademicYear { get; set; }

    public string? PreRequisites { get; set; }

    public string? CoRequisites { get; set; }

    public string? StudentAssessment { get; set; }

    public string? PassingGrade { get; set; }

    public string? PlarMethod { get; set; }

    public int? InstructorId { get; set; }

    public int? PreparedByUserId { get; set; }

    public DateOnly? PreparedDate { get; set; }

    public int? ApprovedByProgramHeadUserId { get; set; }

    public DateOnly? ApprovedByProgramHeadDate { get; set; }

    public int? ApprovedByAcademicChairUserId { get; set; }

    public DateOnly? ApprovedByAcademicChairDate { get; set; }

    public virtual User? ApprovedByAcademicChairUser { get; set; }

    public virtual User? ApprovedByProgramHeadUser { get; set; }

    public virtual User? Instructor { get; set; }

    public virtual User? PreparedByUser { get; set; }

    public virtual ProgramCourse? ProgramCourse { get; set; }
}
