using FluentValidation;
using Terminator.Application.Common;
using Terminator.Application.Common.Errors;
using Terminator.Core.Common;

namespace Terminator.Application.Features.Auth.Register;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.UsernameRequired.Code)
            .WithMessage(ValidationErrors.Auth.UsernameRequired.Message)
            
            .MaximumLength(UserConstants.MaxUsernameLength)
            .WithErrorCode(ValidationErrors.Auth.UsernameTooLong.Code)
            .WithMessage(ValidationErrors.Auth.UsernameTooLong.Message);

        RuleFor(x => x.AuthSalt)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Sync.EncryptedBlob.BlobRequired.Code)
            .WithMessage(ValidationErrors.Sync.EncryptedBlob.BlobRequired.Message)
            
            .Must(Base64ValidationHelper.IsValidBase64)
            .WithErrorCode(ValidationErrors.Sync.EncryptedBlob.BlobInvalidFormat.Code)
            .WithMessage(ValidationErrors.Sync.EncryptedBlob.BlobInvalidFormat.Message);
        
        RuleFor(x => x.AuthSalt)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.AuthSaltRequired.Code)
            .WithMessage(ValidationErrors.Auth.AuthSaltRequired.Message)
            
            .Must(Base64ValidationHelper.IsValidBase64)
            .WithErrorCode(ValidationErrors.Auth.AuthSaltInvalid.Code)
            .WithMessage(ValidationErrors.Auth.AuthSaltInvalid.Message);

        RuleFor(x => x.KeySalt)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.KeySaltRequired.Code)
            .WithMessage(ValidationErrors.Auth.KeySaltRequired.Message)

            .Must(Base64ValidationHelper.IsValidBase64)
            .WithErrorCode(ValidationErrors.Auth.KeySaltInvalid.Code)
            .WithMessage(ValidationErrors.Auth.KeySaltInvalid.Message);

        RuleFor(x => x.LoginKey)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.LoginKeyRequired.Code)
            .WithMessage(ValidationErrors.Auth.LoginKeyRequired.Message)

            .Must(Base64ValidationHelper.IsValidBase64)
            .WithErrorCode(ValidationErrors.Auth.LoginKeyInvalid.Code)
            .WithMessage(ValidationErrors.Auth.LoginKeyInvalid.Message);

        RuleFor(x => x.EncryptedMasterKey)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Auth.MasterKeyRequired.Code)
            .WithMessage(ValidationErrors.Auth.MasterKeyRequired.Message)

            .Must(Base64ValidationHelper.IsValidBase64)
            .WithErrorCode(ValidationErrors.Auth.MasterKeyInvalid.Code)
            .WithMessage(ValidationErrors.Auth.MasterKeyInvalid.Message);
    }
}