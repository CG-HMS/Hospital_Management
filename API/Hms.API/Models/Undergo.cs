using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Undergo
{
    public int Patient { get; set; }

    public int Procedures { get; set; }

    public int Stay { get; set; }

    public DateTime DateUndergoes { get; set; }

    public int Physician { get; set; }

    public int? AssistingNurse { get; set; }

    public virtual Nurse? AssistingNurseNavigation { get; set; }

    public virtual Patient PatientNavigation { get; set; } = null!;

    public virtual Physician PhysicianNavigation { get; set; } = null!;

    public virtual Procedure ProceduresNavigation { get; set; } = null!;

    public virtual Stay StayNavigation { get; set; } = null!;
}
