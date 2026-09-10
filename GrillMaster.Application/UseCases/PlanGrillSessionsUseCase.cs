using GrillMaster.Application.DTOs;
using GrillMaster.Domain.Abstractions;
using GrillMaster.Domain.DTOs;
using GrillMaster.Domain.Entities;

namespace GrillMaster.Application.UseCases;

/// <summary>
/// Orchestrates the conversion of menu requests into a summarized grill plan.
/// </summary>
public sealed class PlanGrillSessionsUseCase(IBinPackingService packingService)
{
    public GrillSummaryDto Execute(IReadOnlyList<MenuRequestDto> requests)
    {
        var menuSummaries = requests.Select(request =>
        {
            var items = ExpandItems(request);
            var sessions = packingService.Pack(items); // Start with the logic
            return new MenuSummaryDto(request.Menu, sessions.Count);
        }).ToList();

        return new GrillSummaryDto(
            Menus: menuSummaries,
            TotalRounds: menuSummaries.Sum(m => m.Rounds));
    }

    // Expand each item type by his quantity into individual units
    private static IEnumerable<GrillItem> ExpandItems(MenuRequestDto request) =>
        request.Items.SelectMany(dto =>
            Enumerable.Range(0, dto.Quantity)
                .Select(_ => new GrillItem(
                    Guid.Parse(dto.Id),
                    dto.Name,
                    dto.Length,
                    dto.Width,
                    quantity: 1)));
}