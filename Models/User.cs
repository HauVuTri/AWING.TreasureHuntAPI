using System;
using System.Collections.Generic;

namespace AWING.TreasureHuntAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TreasureMap> TreasureMaps { get; set; } = new List<TreasureMap>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
