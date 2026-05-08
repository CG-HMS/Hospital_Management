using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Room
{
    public int RoomNumber { get; set; }

    public string RoomType { get; set; } = null!;

    public int BlockFloor { get; set; }

    public int BlockCode { get; set; }

    public bool Unavailable { get; set; }

    public virtual Block Block { get; set; } = null!;

    public virtual ICollection<Stay> Stays { get; set; } = new List<Stay>();
}
