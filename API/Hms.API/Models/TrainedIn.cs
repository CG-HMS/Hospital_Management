using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class TrainedIn
{
    public int Physician { get; set; }

    public int Treatment { get; set; }

    public DateTime CertificationDate { get; set; }

    public DateTime CertificationExpires { get; set; }

    public virtual Physician PhysicianNavigation { get; set; } = null!;

    public virtual Procedure TreatmentNavigation { get; set; } = null!;
}
