using System;
using System.Collections.Generic;

namespace AWING.TreasureHuntAPI.Models;

public partial class TreasureMap
{
    public int MapId { get; set; }

    public int? UserId { get; set; }

    public int RowsCount { get; set; }

    public int ColsCount { get; set; }

    public int P { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ICollection<TreasureCell> TreasureCells { get; set; } = new List<TreasureCell>();

    public virtual User? User { get; set; }
}
