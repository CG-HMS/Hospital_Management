using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Nurse
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string Position { get; set; } = null!;

    public bool Registered { get; set; }

    public int Ssn { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<OnCall> OnCalls { get; set; } = new List<OnCall>();

    public virtual ICollection<Undergo> Undergos { get; set; } = new List<Undergo>();
}
