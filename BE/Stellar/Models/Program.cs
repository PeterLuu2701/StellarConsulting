using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class Program
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<ProgramCourse> ProgramCourses { get; set; } = new List<ProgramCourse>();
}
