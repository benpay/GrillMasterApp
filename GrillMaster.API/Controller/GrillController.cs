using GrillMaster.Application.DTOs;
using GrillMaster.Application.UseCases;
using GrillMaster.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GrillMaster.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class GrillController(
    PlanGrillSessionsUseCase useCase,
    IMenuApiClient menuApiClient) : ControllerBase
{
    /// <summary>
    /// Fetches menus from the external API and returns an optimized grill plan.
    /// </summary>
    [HttpGet("plan")]
    [ProducesResponseType(typeof(GrillPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<IReadOnlyList<GrillPlanDto>>> Plan(
        CancellationToken cancellationToken)
    {
        var menus = await menuApiClient.GetMenusAsync(cancellationToken);

        if (menus.Count == 0)
            return Ok(new GrillSummaryDto([], 0));

        var results = useCase.Execute(menus);
        return Ok(results);
    }
}