using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AWING.TreasureHuntAPI.Models;

public partial class TreasureCell
{
    public int CellId { get; set; }

    public int? MapId { get; set; }

    public int RowNum { get; set; }

    public int ColNum { get; set; }

    public int ChestNumber { get; set; }

    [JsonIgnore]
    public virtual TreasureMap? TreasureMap { get; set; }
}
