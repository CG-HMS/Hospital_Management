using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator
{
    public class UpdateProcedureDtoValidator : AbstractValidator<UpdateProcedureDto>
    {
        public UpdateProcedureDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Cost)
                .GreaterThan(0);
        }
    }
}
