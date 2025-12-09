using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Terminator.Application.Features.Sync;
using Sync = Terminator.Application.Features.Sync;

namespace Terminator.Web.Controllers;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SyncController : ApiControllerBase
{
    private readonly ISender _sender;

    public SyncController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Produces(typeof(EncryptedBlobDto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Sync([FromBody] Sync.Request request)
    {
        var result = await _sender.Send(request);

        if (!result.IsSuccessful) return HandleError(result);

        return Ok(result.Value);
    }
}