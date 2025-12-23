using FluentValidation;
using Terminator.Application.Common.Errors;

namespace Terminator.Application.Features.Auth.Preflight;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.UsernameRequired.Code)
            .WithMessage(ValidationErrors.Auth.UsernameRequired.Message);
    }
}