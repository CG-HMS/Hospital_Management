using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator;

public class NurseDtoValidator : AbstractValidator<NurseDto>
{
    public NurseDtoValidator()
    {
        RuleFor(n => n.EmployeeId).GreaterThan(0);
        RuleFor(n => n.Name).NotEmpty().MaximumLength(30);
        RuleFor(n => n.Position).NotEmpty().MaximumLength(30);
        RuleFor(n => n.Ssn).GreaterThan(0);
    }
}

public class NurseCreateDtoValidator : AbstractValidator<NurseCreateDto>
{
    public NurseCreateDtoValidator()
    {
        RuleFor(n => n.Name).NotEmpty().MaximumLength(30);
        RuleFor(n => n.Position).NotEmpty().MaximumLength(30);
        RuleFor(n => n.Ssn).GreaterThan(0);
    }
}

public class NurseUpdateDtoValidator : AbstractValidator<NurseUpdateDto>
{
    public NurseUpdateDtoValidator()
    {
        RuleFor(n => n.Name).NotEmpty().MaximumLength(30);
        RuleFor(n => n.Position).NotEmpty().MaximumLength(30);
    }
}
