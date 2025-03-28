using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class ProgramCourse
{
    public int Id { get; set; }

    public int? ProgramId { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<CourseOutline> CourseOutlines { get; set; } = new List<CourseOutline>();

    public virtual Program? Program { get; set; }
}
