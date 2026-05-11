using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator;

public class CreatePrescriptionDtoValidator : AbstractValidator<CreatePrescriptionDTO>
{
    public CreatePrescriptionDtoValidator()
    {
        RuleFor(x => x.Physician)
            .GreaterThan(0)
            .WithMessage("Physician ID must be greater than 0");

        RuleFor(x => x.Patient)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0");

        RuleFor(x => x.Medication)
            .GreaterThan(0)
            .WithMessage("Medication ID must be greater than 0");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Date cannot be in the future");

        RuleFor(x => x.Dose)
            .NotEmpty()
            .WithMessage("Dose is required")
            .MinimumLength(2)
            .WithMessage("Dose must be at least 2 characters long")
            .MaximumLength(100)
            .WithMessage("Dose cannot exceed 100 characters")
            .Matches(@"^[a-zA-Z0-9\s\-./()]+$")
            .WithMessage("Dose contains invalid characters");

        RuleFor(x => x.Appointment)
            .GreaterThan(0)
            .WithMessage("Appointment ID must be greater than 0")
            .When(x => x.Appointment.HasValue);
    }
}

public class UpdatePrescriptionDtoValidator : AbstractValidator<UpdatePrescriptionDTO>
{
    public UpdatePrescriptionDtoValidator()
    {
        RuleFor(x => x.Physician)
            .GreaterThan(0)
            .WithMessage("Physician ID must be greater than 0");

        RuleFor(x => x.Patient)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0");

        RuleFor(x => x.Medication)
            .GreaterThan(0)
            .WithMessage("Medication ID must be greater than 0");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Date cannot be in the future");

        RuleFor(x => x.Dose)
            .NotEmpty()
            .WithMessage("Dose is required")
            .MinimumLength(2)
            .WithMessage("Dose must be at least 2 characters long")
            .MaximumLength(100)
            .WithMessage("Dose cannot exceed 100 characters")
            .Matches(@"^[a-zA-Z0-9\s\-./()]+$")
            .WithMessage("Dose contains invalid characters");

        RuleFor(x => x.Appointment)
            .GreaterThan(0)
            .WithMessage("Appointment ID must be greater than 0")
            .When(x => x.Appointment.HasValue);
    }
}
