using System;
using System.Collections.Generic;

namespace ContactManagerApi.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Rol { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
