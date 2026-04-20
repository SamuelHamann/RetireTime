using MediatR;
using Microsoft.AspNetCore.Mvc;
using RetirementTime.Application.Features.Dashboard.NetWorth.SnapshotNetWorth;

namespace RetirementTime.Controllers;

[ApiController]
[Route("api/cronjob")]
public class CronjobController(IMediator mediator) : ControllerBase
{
    [HttpPost("snapshot-networth")]
    public async Task<IActionResult> SnapshotNetWorth(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SnapshotNetWorthCommand(), cancellationToken);

        if (!result.Success)
            return StatusCode(500, result.ErrorMessage);

        return Ok($"Net worth snapshot completed. Scenarios processed: {result.ScenariosProcessed}");
    }
}
