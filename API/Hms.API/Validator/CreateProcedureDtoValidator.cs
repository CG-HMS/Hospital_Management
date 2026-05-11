using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator
{
    public class CreateProcedureDtoValidator : AbstractValidator<CreateProcedureDto>
    {
        public CreateProcedureDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Cost)
                .GreaterThan(0);
        }
    }
}
