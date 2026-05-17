namespace Hms.API.DTOs;

public class NurseDto
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public bool Registered { get; set; }
    public int Ssn { get; set; }
}

public class NurseEditDto
{
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public bool Registered { get; set; }
}

public class NurseCreateDto : NurseEditDto
{
    public int Ssn { get; set; }
}

public class NurseUpdateDto : NurseEditDto
{
}

public class NurseOnCallDto
{
    public int BlockFloor { get; set; }
    public int BlockCode { get; set; }
    public DateTime OnCallStart { get; set; }
    public DateTime OnCallEnd { get; set; }
}

public class NurseTrainedProcedureDto
{
    public int ProcedureCode { get; set; }
    public string ProcedureName { get; set; } = string.Empty;
    public DateTime CertificationDate { get; set; }
    public DateTime CertificationExpires { get; set; }
}
