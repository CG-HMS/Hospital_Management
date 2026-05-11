using FluentValidation;
using Hms.API.DTOs;

namespace Hms.API.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(255);
        }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private static readonly string[] AllowedRoles = ["admin", "physician", "nurse", "patient"];

        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(20)
                .Must(role => AllowedRoles.Contains(role.ToLower()))
                .WithMessage($"Role must be one of: {string.Join(", ", AllowedRoles)}");
        }
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MaximumLength(255);
        }
    }

    public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
    {
        private static readonly string[] AllowedRoles = ["admin", "physician", "nurse", "patient"];

        public UpdateRoleDtoValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(20)
                .Must(role => AllowedRoles.Contains(role.ToLower()))
                .WithMessage($"Role must be one of: {string.Join(", ", AllowedRoles)}");
        }
    }

    public class UpdateStatusDtoValidator : AbstractValidator<UpdateStatusDto>
    {
        public UpdateStatusDtoValidator()
        {
            RuleFor(x => x.IsActive).NotNull();
        }
    }

    public class RoomDtoValidator : AbstractValidator<RoomDto>
    {
        public RoomDtoValidator()
        {
            RuleFor(x => x.RoomNumber).GreaterThan(0);
            RuleFor(x => x.RoomType).NotEmpty().MaximumLength(50);
            RuleFor(x => x.BlockFloor).GreaterThan(0);
            RuleFor(x => x.BlockCode).GreaterThanOrEqualTo(0);
        }
    }

    public class CreateRoomDtoValidator : AbstractValidator<CreateRoomDto>
    {
        public CreateRoomDtoValidator()
        {
            RuleFor(x => x.RoomNumber).GreaterThan(0);
            RuleFor(x => x.RoomType).NotEmpty().MaximumLength(50);
            RuleFor(x => x.BlockFloor).GreaterThan(0);
            RuleFor(x => x.BlockCode).GreaterThanOrEqualTo(0);
        }
    }

    public class UpdateRoomDtoValidator : AbstractValidator<UpdateRoomDto>
    {
        public UpdateRoomDtoValidator()
        {
            RuleFor(x => x.RoomType).NotEmpty().MaximumLength(50);
            RuleFor(x => x.BlockFloor).GreaterThan(0);
            RuleFor(x => x.BlockCode).GreaterThanOrEqualTo(0);
        }
    }

    public class UpdateAvailabilityDtoValidator : AbstractValidator<UpdateAvailabilityDto>
    {
        public UpdateAvailabilityDtoValidator()
        {
            RuleFor(x => x.Unavailable).NotNull();
        }
    }
}
