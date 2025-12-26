using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Terminator.Core.Common;
using Terminator.Core.Result;
using Terminator.Web.DTOs;

namespace Terminator.Web.Controllers;

public class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleError(Result result)
    {
        if (result.IsSuccessful)
            throw new InvalidOperationException("Result is successful.");

        var response = ToErrorResponse(result);
        return result.ErrorType switch
        {
            ErrorType.Validation => BadRequest(response),
            ErrorType.NotFound => NotFound(response),
            ErrorType.Unauthorized => Unauthorized(response),
            _ => throw new ArgumentOutOfRangeException(nameof(ErrorType))
        };
    }

    protected Guid? TryObtainUserId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            
        if (idClaim is null || !Guid.TryParse(idClaim.Value, out var id))
        {
            return null;
        }
            
        return id;
    }

    protected string? TryObtainUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role);
            
        if (string.IsNullOrWhiteSpace(roleClaim?.Value))
        {
            return null;
        }

        return roleClaim.Value;
    }

    private ErrorResponse ToErrorResponse(Result result)
    {
        return new ErrorResponse
        {
            Errors = result.Errors.Select(x => new ErrorDto(x.Code, x.Message))
        };
    }
}