using FluentValidation;
using Terminator.Application.Common.Errors;

namespace Terminator.Application.Features.User.Delete;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.User.UserIdRequired.Code)
            .WithMessage(ValidationErrors.User.UserIdRequired.Message);
    }
}