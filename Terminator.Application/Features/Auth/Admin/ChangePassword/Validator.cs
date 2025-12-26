using FluentValidation;
using Terminator.Application.Common.Errors;
using Terminator.Core.Common;

namespace Terminator.Application.Features.Auth.Admin.ChangePassword;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.Admin.UsernameRequired.Code)
            .WithMessage(ValidationErrors.Auth.Admin.UsernameRequired.Message);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.Admin.PasswordRequired.Code)
            .WithMessage(ValidationErrors.Auth.Admin.PasswordRequired.Message)

            .MaximumLength(AdminConstants.MaxPasswordLength)
            .WithErrorCode(ValidationErrors.Auth.Admin.PasswordTooLong.Code)
            .WithMessage(ValidationErrors.Auth.Admin.PasswordTooLong.Message);
    }
}