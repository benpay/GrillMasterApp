namespace GrillMaster.Application.DTOs;

/// <summary>
/// Summary of the grill plan (Menu XX: X rounds / Total: X rounds).
/// </summary>
public sealed record GrillSummaryDto(
    IReadOnlyList<MenuSummaryDto> Menus,
    int TotalRounds);

public sealed record MenuSummaryDto(
    string Menu,
    int Rounds);