using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Block
{
    public int BlockFloor { get; set; }

    public int BlockCode { get; set; }

    public virtual ICollection<OnCall> OnCalls { get; set; } = new List<OnCall>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
