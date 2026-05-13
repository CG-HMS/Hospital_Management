using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator;

public class CreateStayDtoValidator : AbstractValidator<CreateStayDTO>
{
    public CreateStayDtoValidator()
    {
        RuleFor(x => x.Patient)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0");

        RuleFor(x => x.Room)
            .GreaterThan(0)
            .WithMessage("Room ID must be greater than 0");

        RuleFor(x => x.StayStart)
            .NotEmpty()
            .WithMessage("Stay start date is required");

        RuleFor(x => x.StayEnd)
            .NotEmpty()
            .WithMessage("Stay end date is required");

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("Stay end date must be after stay start date");

        RuleFor(x => x.StayStart)
            .LessThanOrEqualTo(DateTime.Now.AddDays(365))
            .WithMessage("Stay start date cannot be more than 1 year in the future");
    }

    private bool HaveValidDateRange(CreateStayDTO stay)
    {
        return stay.StayEnd > stay.StayStart;
    }
}

public class UpdateStayDtoValidator : AbstractValidator<UpdateStayDTO>
{
    public UpdateStayDtoValidator()
    {
        RuleFor(x => x.Patient)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0");

        RuleFor(x => x.Room)
            .GreaterThan(0)
            .WithMessage("Room ID must be greater than 0");

        RuleFor(x => x.StayStart)
            .NotEmpty()
            .WithMessage("Stay start date is required");

        RuleFor(x => x.StayEnd)
            .NotEmpty()
            .WithMessage("Stay end date is required");

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("Stay end date must be after stay start date");

        RuleFor(x => x.StayStart)
            .LessThanOrEqualTo(DateTime.Now.AddDays(365))
            .WithMessage("Stay start date cannot be more than 1 year in the future");
    }

    private bool HaveValidDateRange(UpdateStayDTO stay)
    {
        return stay.StayEnd > stay.StayStart;
    }
}
