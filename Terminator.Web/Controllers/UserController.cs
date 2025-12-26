using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Terminator.Core.Common;
using Delete = Terminator.Application.Features.User.Delete;
using ListAll = Terminator.Application.Features.User.ListAll;

namespace Terminator.Web.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class UserController(ISender sender) : ApiControllerBase
{
    [HttpDelete("{userId:guid?}")]
    [Authorize]
    //[Produces(typeof(Delete.Response))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid? userId)
    {
        var id = TryObtainUserId();
        var role = TryObtainUserRole();
        if (id is null || role is null || (role == RoleType.UserRole && userId != id))
        {
            return Unauthorized();
        }

        userId ??= id;
        
        var result = await sender.Send(new Delete.Request(userId.Value));

        if (!result.IsSuccessful) return HandleError(result);

        return NoContent();
    }
    
    [HttpGet("list")]
    [Authorize(Roles = RoleType.AdminRole)]
    [Produces(typeof(ListAll.Response))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ListAll()
    {
        var result = await sender.Send(new ListAll.Request());

        if (!result.IsSuccessful) return HandleError(result);

        return Ok(result.Value);
    }
}