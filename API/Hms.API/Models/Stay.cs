using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Stay
{
    public int StayId { get; set; }

    public int Patient { get; set; }

    public int Room { get; set; }

    public DateTime StayStart { get; set; }

    public DateTime StayEnd { get; set; }

    public virtual Patient PatientNavigation { get; set; } = null!;

    public virtual Room RoomNavigation { get; set; } = null!;

    public virtual ICollection<Undergo> Undergos { get; set; } = new List<Undergo>();
}
