using FluentValidation;
using Terminator.Application.Common;
using Terminator.Application.Common.Errors;

namespace Terminator.Application.Features.Auth.Login;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.UsernameRequired.Code)
            .WithMessage(ValidationErrors.Auth.UsernameRequired.Message);

        RuleFor(x => x.LoginKey)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.LoginKeyRequired.Code)
            .WithMessage(ValidationErrors.Auth.LoginKeyRequired.Message)

            .Must(Base64ValidationHelper.IsValidBase64)
            .WithErrorCode(ValidationErrors.Auth.LoginKeyInvalid.Code)
            .WithMessage(ValidationErrors.Auth.LoginKeyInvalid.Message);
    }
}