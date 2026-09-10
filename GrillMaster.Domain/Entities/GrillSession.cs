namespace GrillMaster.Domain.Entities;

/// <summary>
/// Represents one grilling session.
/// </summary>
public sealed class GrillSession
{
    public int SessionNumber { get; }

    /// <summary>Items placed during this session, key by item name.</summary>
    public IReadOnlyList<PlacedItem> PlacedItems { get; }

    public GrillSession(int sessionNumber, IReadOnlyList<PlacedItem> placedItems)
    {
        SessionNumber = sessionNumber;
        PlacedItems = placedItems;
    }
}

/// <summary>
/// An item instance placed at a specific position in a session.
/// </summary>
public sealed record PlacedItem(
    string Name,
    int X,
    int Y,
    int Length,
    int Width,
    bool IsRotated);