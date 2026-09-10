using System.Net.Http.Json;
using GrillMaster.Domain.DTOs;

namespace GrillMaster.Infrastructure.ExternalApi;

/// <summary>
/// HTTP client to fetch menu data from the external barbecue API.
/// </summary>
public sealed class MenuApiClient(HttpClient httpClient) : IMenuApiClient
{
    public async Task<IReadOnlyList<MenuRequestDto>> GetMenusAsync(CancellationToken cancellationToken = default)
    {
        var result = await httpClient.GetFromJsonAsync<List<MenuRequestDto>>(
            requestUri: string.Empty,
            cancellationToken: cancellationToken);

        return result ?? [];
    }
}