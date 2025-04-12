namespace Stellar.DTOs
{
    public class CourseOutlineDto
    {
        public int ProgramCourseId { get; set; }
        public string AcademicYear { get; set; }
        public string? PreRequisites { get; set; }
        public string? CoRequisites { get; set; }
        public string StudentAssessment { get; set; }
        public string PassingGrade { get; set; }
        public string? PlarMethod { get; set; }
        public int InstructorId { get; set; }
        public int PreparedByUserId { get; set; }
        public int? ApprovedByProgramHeadUserId { get; set; }
        public int? ApprovedByAcademicChairUserId { get; set; }
        public List<LearningOutcomeDto> LearningOutcomes { get; set; }
    }

    public class LearningStepDto
    {
        public string LearningText { get; set; }
    }

    public class LearningOutcomeDto
    {
        public string OutcomeText { get; set; }
        public string LearningActivities { get; set; }
        public List<LearningStepDto> LearningSteps { get; set; }
    }
}
