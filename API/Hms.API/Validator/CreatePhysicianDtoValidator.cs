using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator
{
    public class CreatePhysicianDtoValidator :AbstractValidator<CreatePhysicianDto>
    {
        public CreatePhysicianDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Position)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Ssn)
                .GreaterThan(0);
        }
    }
}
