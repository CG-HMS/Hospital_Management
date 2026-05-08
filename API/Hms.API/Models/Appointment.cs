using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int Patient { get; set; }

    public int? PrepNurse { get; set; }

    public int Physician { get; set; }

    public DateTime Starto { get; set; }

    public DateTime Endo { get; set; }

    public string ExaminationRoom { get; set; } = null!;

    public virtual Patient PatientNavigation { get; set; } = null!;

    public virtual Physician PhysicianNavigation { get; set; } = null!;

    public virtual Nurse? PrepNurseNavigation { get; set; }

    public virtual ICollection<Prescribe> Prescribes { get; set; } = new List<Prescribe>();
}
