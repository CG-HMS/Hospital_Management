using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Medication
{
    public int Code { get; set; }

    public string Name { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Prescribe> Prescribes { get; set; } = new List<Prescribe>();
}
