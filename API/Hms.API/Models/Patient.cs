using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Patient
{
    public int Ssn { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int InsuranceId { get; set; }

    public int Pcp { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Physician PcpNavigation { get; set; } = null!;

    public virtual ICollection<Prescribe> Prescribes { get; set; } = new List<Prescribe>();

    public virtual ICollection<Stay> Stays { get; set; } = new List<Stay>();

    public virtual ICollection<Undergo> Undergos { get; set; } = new List<Undergo>();
}
