using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator;

public class AppointmentDtoValidator : AbstractValidator<AppointmentDto>
{
    public AppointmentDtoValidator()
    {
        RuleFor(a => a.AppointmentId).GreaterThan(0);
        RuleFor(a => a.Patient).GreaterThan(0);
        RuleFor(a => a.Physician).GreaterThan(0);
        RuleFor(a => a.Starto).LessThan(a => a.Endo);
        RuleFor(a => a.ExaminationRoom).NotEmpty();
    }
}
