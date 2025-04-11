using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class LearningOutcome
{
    public int Id { get; set; }

    public string? OutcomeText { get; set; }

    public int? CourseId { get; set; }

    public string? LearningActivities { get; set; }

    public virtual Course? Course { get; set; }

    [JsonIgnore]
    public virtual ICollection<LearningStep> LearningSteps { get; set; } = new List<LearningStep>();
}
