using System;
using System.Collections.Generic;

namespace ContactManagerApi.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public Guid IdUser { get; set; }

    public int IdRol { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
