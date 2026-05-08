using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class OnCall
{
    public int Nurse { get; set; }

    public int BlockFloor { get; set; }

    public int BlockCode { get; set; }

    public DateTime OnCallStart { get; set; }

    public DateTime OnCallEnd { get; set; }

    public virtual Block Block { get; set; } = null!;

    public virtual Nurse NurseNavigation { get; set; } = null!;
}
