using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class LearningStep
{
    public int Id { get; set; }

    public string? LearningText { get; set; }

    public int LearningOutcomeId { get; set; }

    public virtual LearningOutcome LearningOutcome { get; set; }
}
