using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class Program
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? SchoolId { get; set; }

    public int? DegreeId { get; set; }

    public virtual Degree? Degree { get; set; }

    public virtual ICollection<ProgramCourse> ProgramCourses { get; set; } = new List<ProgramCourse>();

    public virtual School? School { get; set; }
}