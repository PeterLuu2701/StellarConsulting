using System;
using System.Collections.Generic;

namespace Stellar.Models;

public partial class Degree
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Program> Programs { get; set; } = new List<Program>();
}
