namespace Hms.MVC.Models.Nurse;

public class NurseViewModel
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public bool Registered { get; set; }
    public int? Ssn { get; set; }
}
