using FluentValidation;
using Hms.API.DTOs.Medication;

namespace Hms.API.Validator;

public class MedicationValidator : AbstractValidator<MedicationRequestDto>
{
    public MedicationValidator()
    {
        RuleFor(x => x.Code)
            .GreaterThan(0)
            .WithMessage("Code must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Brand)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(250);
    }
}