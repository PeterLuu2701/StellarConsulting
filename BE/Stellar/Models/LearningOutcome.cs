using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class LearningOutcome
{
    public int Id { get; set; }

    public string? OutcomeText { get; set; }

    public string? LearningActivities { get; set; }

    public int CourseOutlineId { get; set; }

    public virtual CourseOutline CourseOutline { get; set; }

    public virtual ICollection<LearningStep> LearningSteps { get; set; } = new List<LearningStep>();
}
