using GrillMaster.Domain.DTOs;

namespace GrillMaster.API.Mappers;

/// <summary>
/// Mapping input into Application DTOs allow us to keeps the 
/// controller and the mapping logic testable with isolation.
/// </summary>
public static class MenuRequestMapper
{
    public static IReadOnlyList<MenuRequestDto> FromRequest(
        IReadOnlyList<MenuRequestDto> requests) => requests;
}