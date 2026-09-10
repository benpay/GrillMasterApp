namespace GrillMaster.Domain.ValueObjects;

/// <summary>
/// Represents the position of an item on the grill grid.
/// </summary>
public sealed record Placement(int X, int Y, int Length, int Width, bool IsRotated);