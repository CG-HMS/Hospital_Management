using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validator
{
    public static class DtoValidators
    {
        public class CreatePhysicianDtoValidator : AbstractValidator<CreatePhysicianDto>
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
    }
}
