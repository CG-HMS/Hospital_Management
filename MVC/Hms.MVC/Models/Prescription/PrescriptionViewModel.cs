namespace Hms.MVC.Models.Prescription;

public class PrescriptionViewModel
{
    public int Physician { get; set; }
    public int Patient { get; set; }
    public int Medication { get; set; }
    public DateTime Date { get; set; }
    public int Appointment { get; set; }
    public string Dose { get; set; } = string.Empty;
}
