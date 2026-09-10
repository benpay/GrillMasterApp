namespace GrillMaster.Application.DTOs;

/// <summary>Full grilling plan returned to the caller.</summary>
public sealed record GrillPlanDto(
    string Menu,
    int TotalSessions,
    IReadOnlyList<SessionDto> Sessions);

public sealed record SessionDto(
    int SessionNumber,
    IReadOnlyList<PlacedItemDto> Items);

public sealed record PlacedItemDto(
    string Name,
    int X,
    int Y,
    int Length,
    int Width,
    bool IsRotated);