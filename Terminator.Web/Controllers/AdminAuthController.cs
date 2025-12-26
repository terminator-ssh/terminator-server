using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Register =  Terminator.Application.Features.Auth.Admin.Register;
using Login =  Terminator.Application.Features.Auth.Admin.Login;
using ChangePassword =  Terminator.Application.Features.Auth.Admin.ChangePassword;
using Terminator.Core.Common;
using Terminator.Web.DTOs;
using Terminator.Web.DTOs.Auth.Admin;

namespace Terminator.Web.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/auth/admin")]
[ApiController]
public class AdminAuthController(ISender sender) : ApiControllerBase
{
    [HttpPost("register")]
    [Authorize(Roles = RoleType.AdminRole)]
    [ProducesResponseType(typeof(AdminAuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] AdminRegisterRequestDto webRequest)
    {
        var request = new Register.Request(
            webRequest.Username, 
            webRequest.Password);
        
        var result = await sender.Send(request);
        
        if (!result.IsSuccessful) return HandleError(result);
        
        return Ok(result.Value);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AdminAuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] AdminLoginRequestDto webRequest)
    {
        var request = new Login.Request(webRequest.Username, webRequest.Password);
        
        var result = await sender.Send(request);
        
        if (!result.IsSuccessful) return HandleError(result);
        
        return Ok(result.Value);
    }
    
    [HttpPost("changePassword")]
    [Authorize(Roles = RoleType.AdminRole)]
    [ProducesResponseType(typeof(AdminAuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] AdminPasswordChangeRequestDto webRequest)
    {
        var id = TryObtainUserId();
        if (id is null)
        {
            return Unauthorized();
        }
        
        var request = new ChangePassword.Request(id.Value, webRequest.NewPassword);
        
        var result = await sender.Send(request);
        
        if (!result.IsSuccessful) return HandleError(result);
        
        return Ok(result.Value);
    }

}