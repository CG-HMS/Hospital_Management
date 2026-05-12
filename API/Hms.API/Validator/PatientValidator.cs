using FluentValidation;
using Hms.API.DTOs.Patient;

namespace Hms.API.Validator;

public class PatientValidator : AbstractValidator<PatientRequestDto>
{
    public PatientValidator()
    {
        RuleFor(x => x.Ssn)
            .GreaterThan(0)
            .WithMessage("SSN must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(15);
            RuleFor(x => x.InsuranceId)
            .GreaterThan(0);

        RuleFor(x => x.Pcp)
            .GreaterThan(0);
    }
}