namespace GrillMaster.Infrastructure.Packing;

/// <summary>
/// Represents a free rectangular space available on the grill surface.
/// </summary>
internal sealed record FreeRectangle(int X, int Y, int Width, int Length);