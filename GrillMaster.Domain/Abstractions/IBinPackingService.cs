using GrillMaster.Domain.Entities;

namespace GrillMaster.Domain.Abstractions;

/// <summary>
/// Contract for this grill algorithm over a fixed grill surface.
/// </summary>
public interface IBinPackingService
{
    /// <summary>
    /// Packs a flat list of individual items (when it is already expanded by quantity)
    /// into the minimum number of grill sessions.
    /// </summary>
    IReadOnlyList<GrillSession> Pack(IEnumerable<GrillItem> items);
}