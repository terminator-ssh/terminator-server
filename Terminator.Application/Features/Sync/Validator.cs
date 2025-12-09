using FluentValidation;
using Terminator.Core.Common.Errors;

namespace Terminator.Application.Features.Sync;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleForEach(x => x.Blobs)
            .SetValidator(new EncryptedBlobDtoValidator());

        RuleFor(x => x.LastSyncTime)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow)
            .WithErrorCode(DomainErrors.Sync.EncryptedBlob.InvalidTimestamp.Code)
            .WithMessage(DomainErrors.Sync.EncryptedBlob.InvalidTimestamp.Message);
    }
}