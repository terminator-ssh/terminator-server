using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Terminator.Web.DTOs;
using Terminator.Web.DTOs.Auth;
using Login = Terminator.Application.Features.Auth.Login;
using Register = Terminator.Application.Features.Auth.Register;
using Preflight = Terminator.Application.Features.Auth.Preflight;

namespace Terminator.Web.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController(ISender sender) : ApiControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto webRequest)
    {
        var request = new Register.Request(
            webRequest.Username, 
            webRequest.AuthSalt, 
            webRequest.KeySalt,
            webRequest.EncryptedMasterKey, 
            webRequest.LoginKey);
        
        var result = await sender.Send(request);
        
        if (!result.IsSuccessful) return HandleError(result);
        
        return Ok(result.Value);
    }

    [HttpPost("preflight")]
    [ProducesResponseType(typeof(PreflightResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Preflight([FromBody] PreflightRequestDto webRequest)
    {
        var request = new Preflight.Request(webRequest.Username);
        
        var result = await sender.Send(request);
        
        if (!result.IsSuccessful) return HandleError(result);
        
        return Ok(result.Value);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto webRequest)
    {
        var request = new Login.Request(webRequest.Username, webRequest.LoginKey);
        
        var result = await sender.Send(request);
        
        if (!result.IsSuccessful) return HandleError(result);
        
        return Ok(result.Value);
    }
}