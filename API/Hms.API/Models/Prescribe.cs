using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Prescribe
{
    public int Physician { get; set; }

    public int Patient { get; set; }

    public int Medication { get; set; }

    public DateTime Date { get; set; }

    public int? Appointment { get; set; }

    public string Dose { get; set; } = null!;

    public virtual Appointment? AppointmentNavigation { get; set; }

    public virtual Medication MedicationNavigation { get; set; } = null!;

    public virtual Patient PatientNavigation { get; set; } = null!;

    public virtual Physician PhysicianNavigation { get; set; } = null!;
}
