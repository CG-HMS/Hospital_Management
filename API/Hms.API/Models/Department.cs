using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public int Head { get; set; }

    public virtual ICollection<AffiliatedWith> AffiliatedWiths { get; set; } = new List<AffiliatedWith>();

    public virtual Physician HeadNavigation { get; set; } = null!;
}
