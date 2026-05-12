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
