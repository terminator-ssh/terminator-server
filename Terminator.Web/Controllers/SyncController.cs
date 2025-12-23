using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Terminator.Application.Features.Sync;
using Terminator.Web.DTOs.Sync;
using Sync = Terminator.Application.Features.Sync;

namespace Terminator.Web.Controllers;

[Authorize]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SyncController(ISender sender) : ApiControllerBase
{
    [HttpPost]
    [Produces(typeof(EncryptedBlobDto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Sync([FromBody] SyncRequestDto webRequest)
    {
        var id = TryObtainUserId();
        if (id is null)
        {
            return Unauthorized();
        }
        
        var request = new Sync.Request(webRequest.Blobs, webRequest.LastSyncTime, id.Value);
        
        var result = await sender.Send(request);

        if (!result.IsSuccessful) return HandleError(result);

        return Ok(result.Value);
    }
}