using FluentValidation;
using Hms.API.DTOs.Medication;

namespace Hms.API.Validator;

    public class UpdateMedicationDtoValidator : AbstractValidator<UpdateMedicationDto>
    {
        public UpdateMedicationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Brand)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
