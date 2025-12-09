using FluentValidation;
using Terminator.Application.Common;
using Terminator.Core.Common;
using DomainErrors = Terminator.Core.Common.Errors.DomainErrors;
using ValidationErrors = Terminator.Application.Common.Errors.ValidationErrors;

namespace Terminator.Application.Features.Sync;

public class EncryptedBlobDtoValidator : AbstractValidator<EncryptedBlobDto>
{
    public EncryptedBlobDtoValidator()
    {   
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Sync.EncryptedBlob.IdRequired.Code)
            .WithMessage(ValidationErrors.Sync.EncryptedBlob.IdRequired.Message);

        RuleFor(x => x.Blob)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Sync.EncryptedBlob.BlobRequired.Code)
            .WithMessage(ValidationErrors.Sync.EncryptedBlob.BlobRequired.Message)
            
            .Must(Base64ValidationHelper.IsValidBase64)
            .WithErrorCode(ValidationErrors.Sync.EncryptedBlob.BlobInvalidFormat.Code)
            .WithMessage(ValidationErrors.Sync.EncryptedBlob.BlobInvalidFormat.Message);
        
        RuleFor(x => x.Iv)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Sync.EncryptedBlob.IvRequired.Code)
            .WithMessage(ValidationErrors.Sync.EncryptedBlob.IvRequired.Message)
            
            .Must(iv => Base64ValidationHelper
                .IsValidBase64AndLength(iv, CryptoConstants.AesGcmIvLengthBytes))
            .WithErrorCode(ValidationErrors.Sync.EncryptedBlob.IvInvalidFormat.Code)
            .WithMessage(ValidationErrors.Sync.EncryptedBlob.IvInvalidFormat.Message);

        RuleFor(x => x.UpdatedAt)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow)
            .WithErrorCode(DomainErrors.Sync.EncryptedBlob.InvalidTimestamp.Code)
            .WithMessage(DomainErrors.Sync.EncryptedBlob.InvalidTimestamp.Message);
    }
}