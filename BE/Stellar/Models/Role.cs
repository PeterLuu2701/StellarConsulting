using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stellar.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
