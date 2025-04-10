using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? RoleId { get; set; }

    public string? Password { get; set; }

    [JsonIgnore]
    public virtual ICollection<CourseOutline> CourseOutlineApprovedByAcademicChairUsers { get; set; } = new List<CourseOutline>();

    [JsonIgnore]
    public virtual ICollection<CourseOutline> CourseOutlineApprovedByProgramHeadUsers { get; set; } = new List<CourseOutline>();

    [JsonIgnore]
    public virtual ICollection<CourseOutline> CourseOutlineInstructors { get; set; } = new List<CourseOutline>();

    [JsonIgnore]
    public virtual ICollection<CourseOutline> CourseOutlinePreparedByUsers { get; set; } = new List<CourseOutline>();

    public virtual Role? Role { get; set; }
}
