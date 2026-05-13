using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validators
{

    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(255);
        }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private static readonly string[] AllowedRoles = ["admin", "physician", "nurse", "patient"];

        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(255);

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(r => AllowedRoles.Contains(r.ToLower()))
                .WithMessage($"Role must be one of: {string.Join(", ", AllowedRoles)}.");
        }
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters.")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password must be different from current password.");
        }
    }

    public class RoomWriteDtoValidator : AbstractValidator<RoomWriteDto>
    {
        public RoomWriteDtoValidator()
        {
            RuleFor(x => x.RoomType)
                .NotEmpty().WithMessage("Room type is required.")
                .MaximumLength(30).WithMessage("Room type cannot exceed 30 characters.");

            RuleFor(x => x.BlockFloor)
                .GreaterThan(0).WithMessage("Block floor must be a positive integer.");

            RuleFor(x => x.BlockCode)
                .GreaterThanOrEqualTo(0).WithMessage("Block code must be zero or positive.");
        }
    }
}