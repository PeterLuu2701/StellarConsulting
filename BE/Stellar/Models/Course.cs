using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class Course
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Units { get; set; }

    public int? Hours { get; set; }

    [JsonIgnore]
    public virtual ICollection<LearningOutcome> LearningOutcomes { get; set; } = new List<LearningOutcome>();

    [JsonIgnore]
    public virtual ICollection<ProgramCourse> ProgramCourses { get; set; } = new List<ProgramCourse>();
}
