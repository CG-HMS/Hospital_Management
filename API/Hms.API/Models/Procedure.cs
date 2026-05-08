using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Procedure
{
    public int Code { get; set; }

    public string Name { get; set; } = null!;

    public float Cost { get; set; }

    public virtual ICollection<TrainedIn> TrainedIns { get; set; } = new List<TrainedIn>();

    public virtual ICollection<Undergo> Undergos { get; set; } = new List<Undergo>();
}
