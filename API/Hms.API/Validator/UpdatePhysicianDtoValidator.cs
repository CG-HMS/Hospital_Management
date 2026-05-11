using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator
{
    public class UpdatePhysicianDtoValidator : AbstractValidator<UpdatePhysicianDto>
    {
        public UpdatePhysicianDtoValidator()
        {
            RuleFor(x => x.Name)
    .MaximumLength(100)
    .When(x => x.Name != null);

            RuleFor(x => x.Position)
                .MaximumLength(100)
                .When(x => x.Position != null);

            RuleFor(x => x.Ssn)
                .GreaterThan(0)
                .When(x => x.Ssn.HasValue);
        }
    }
}
