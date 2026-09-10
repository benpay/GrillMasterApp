using GrillMaster.Domain.Constants;

namespace GrillMaster.Domain.DTOs;

/// <summary>
/// Contract for fetching menu data from an external API.
/// </summary>
public interface IMenuApiClient
{
    Task<IReadOnlyList<MenuRequestDto>> GetMenusAsync(CancellationToken cancellationToken = default);
}