using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AWING.TreasureHuntAPI.Models;

public partial class Result
{
    public int ResultId { get; set; }

    public int? MapId { get; set; }

    public double FuelUsed { get; set; }

    public DateTime? CalculatedAt { get; set; }

    [JsonIgnore]
    public virtual TreasureMap? TreasureMap { get; set; }
}
