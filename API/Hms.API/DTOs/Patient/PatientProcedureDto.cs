namespace Hms.API.DTOs.Patient;

public class PatientProcedureDto
{
    public int ProcedureCode { get; set; }
    public string ProcedureName { get; set; } = string.Empty;
    public DateTime DateUndergoes { get; set; }
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
    public int? AssistingNurseId { get; set; }
    public string? AssistingNurseName { get; set; }
}
