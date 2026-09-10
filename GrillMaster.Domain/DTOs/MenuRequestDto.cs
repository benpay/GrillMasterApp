namespace GrillMaster.Domain.DTOs;

public sealed record MenuRequestDto(
    string Id,
    string Menu,
    IReadOnlyList<GrillItemDto> Items);

public sealed record GrillItemDto(
    string Id,
    string Name,
    int Length,
    int Width,
    string Duration,
    int Quantity);