namespace Hms.MVC.Models.Patient;

// ── Summary stats (from GET /api/patient/{ssn}/dashboard) ─────────────────
public class PatientDashboardStatsDto
{
    public int AppointmentCount { get; set; }
    public int MedicationCount { get; set; }
    public int StayCount { get; set; }
    public int ProcedureCount { get; set; }
    public DateTime? LastAppointmentDate { get; set; }
}

// ── Profile (from GET /api/patient/profile) ────────────────────────────────
public class PatientProfileDto
{
    public int Ssn { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int InsuranceId { get; set; }
    public int Pcp { get; set; }
}

// ── Appointments (from GET /api/patient/{ssn}/appointments) ───────────────
public class PatientAppointmentDto
{
    public int AppointmentId { get; set; }
    public DateTime Starto { get; set; }
    public DateTime Endo { get; set; }
    public string ExaminationRoom { get; set; } = string.Empty;
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
}

// ── Medications (from GET /api/patient/{ssn}/medications) ─────────────────
public class PatientMedicationDto
{
    public int MedicationCode { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public string Dose { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
}

// ── Procedures (from GET /api/patient/{ssn}/procedures) ───────────────────
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

// ── Stays (from GET /api/patient/{ssn}/stays) ──────────────────────────────
public class PatientStayDto
{
    public int StayId { get; set; }
    public DateTime StayStart { get; set; }
    public DateTime StayEnd { get; set; }
    public int RoomNumber { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public int BlockFloor { get; set; }
    public int BlockCode { get; set; }
}

// ── Main ViewModel assembled by DashboardController ───────────────────────
public class PatientDashboardViewModel
{
    public string Username { get; set; } = string.Empty;
    public PatientProfileDto Profile { get; set; } = new();
    public PatientDashboardStatsDto Stats { get; set; } = new();
    public List<PatientAppointmentDto> Appointments { get; set; } = new();
    public List<PatientMedicationDto> Medications { get; set; } = new();
    public List<PatientProcedureDto> Procedures { get; set; } = new();
    public List<PatientStayDto> Stays { get; set; } = new();
}
