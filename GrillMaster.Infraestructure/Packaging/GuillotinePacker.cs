using GrillMaster.Domain.Abstractions;
using GrillMaster.Domain.Constants;
using GrillMaster.Domain.Entities;

namespace GrillMaster.Infrastructure.Packing;

/// <summary>
/// Implements 2D bin packing using the Guillotine algorithm with
/// Best Short Side Fit (BSSF) heuristic and item rotation support.
/// 
/// Strategy:
/// - Items are sorted by area descending (largest first) for better fit.
/// - For each item, the free rectangle that wastes the least short-side space is chosen.
/// - After placing an item, the used rectangle is split into two new free rectangles (guillotine cut).
/// - If an item does not fit in any orientation, a new grill session is started.
/// </summary>
public sealed class GuillotinePacker : IBinPackingService
{
    private readonly int _grillWidth = GrillConstants.GrillWidth;
    private readonly int _grillLength = GrillConstants.GrillLength;

    public IReadOnlyList<GrillSession> Pack(IEnumerable<GrillItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        // Sort largest area first — improves packing efficiency
        var sortedItems = items
            .OrderByDescending(i => i.Area)
            .ToList();

        var sessions = new List<GrillSession>();
        var remainingItems = new Queue<GrillItem>(sortedItems);
        int sessionNumber = 1;

        while (remainingItems.Count > 0)
        {
            var (placedItems, unplaced) = PackSingleSession(remainingItems, sessionNumber);
            sessions.Add(new GrillSession(sessionNumber, placedItems));
            sessionNumber++;

            // Unplaced items become the input for the next session
            remainingItems = new Queue<GrillItem>(unplaced);
        }

        return sessions;
    }

    /// <summary>
    /// Attempts to pack as many items as possible into a single grill session.
    /// Returns placed items and any items that did not fit.
    /// </summary>
    private (IReadOnlyList<PlacedItem> placed, IReadOnlyList<GrillItem> unplaced)
        PackSingleSession(Queue<GrillItem> items, int sessionNumber)
    {
        var freeRects = new List<FreeRectangle>
        {
            new(X: 0, Y: 0, Width: _grillWidth, Length: _grillLength)
        };

        var placed = new List<PlacedItem>();
        var unplaced = new List<GrillItem>();
        var pending = items.ToList();

        foreach (var item in pending)
        {
            var result = TryPlace(item, freeRects);

            if (result is null)
            {
                unplaced.Add(item);
                continue;
            }

            var (rect, isRotated) = result.Value;
            int usedWidth = isRotated ? item.Length : item.Width;
            int usedLength = isRotated ? item.Width : item.Length;

            placed.Add(new PlacedItem(
                Name: item.Name,
                X: rect.X,
                Y: rect.Y,
                Length: usedLength,
                Width: usedWidth,
                IsRotated: isRotated));

            SplitFreeRectangle(freeRects, rect, usedWidth, usedLength);
        }

        return (placed, unplaced);
    }

    /// <summary>
    /// Finds the best free rectangle for the item.
    /// Also tries rotating the item 90° the area is reduced.
    /// Returns null if the item does not fit anywhere.
    /// </summary>
    private static (FreeRectangle rect, bool isRotated)?
        TryPlace(GrillItem item, List<FreeRectangle> freeRects)
    {
        FreeRectangle? bestRect = null;
        bool bestRotated = false;
        int bestShortSideWaste = GrillConstants.GrillLength * GrillConstants.GrillWidth;

        foreach (var rect in freeRects)
        {
            // Try normal orientation
            if (item.Width <= rect.Width && item.Length <= rect.Length)
            {
                int waste = ShortSideWaste(rect, item.Width, item.Length);
                if (waste < bestShortSideWaste)
                {
                    bestShortSideWaste = waste;
                    bestRect = rect;
                    bestRotated = false;
                }
            }

            // Try rotated 90°
            if (item.Length <= rect.Width && item.Width <= rect.Length)
            {
                int waste = ShortSideWaste(rect, item.Length, item.Width);
                if (waste < bestShortSideWaste)
                {
                    bestShortSideWaste = waste;
                    bestRect = rect;
                    bestRotated = true;
                }
            }
        }

        return bestRect is null ? null : (bestRect, bestRotated);
    }

    /// <summary>
    /// Calculates short-side waste: the smaller leftover dimension after placing the item.
    /// Lower value = better fit.
    /// </summary>
    private static int ShortSideWaste(FreeRectangle rect, int usedWidth, int usedLength)
        => Math.Min(rect.Width - usedWidth, rect.Length - usedLength);

    /// <summary>
    /// Splits the used free rectangle into two new free rectangles (guillotine cut).
    /// Uses the "longer axis" split rule for better remaining space.
    /// </summary>
    private static void SplitFreeRectangle(
        List<FreeRectangle> freeRects,
        FreeRectangle used,
        int usedWidth,
        int usedLength)
    {
        freeRects.Remove(used);

        int rightWidth = used.Width - usedWidth;
        int topLength = used.Length - usedLength;

        // Split horizontally or vertically based on which leftover is larger
        if (rightWidth > topLength)
        {
            // Right rectangle (full height)
            if (rightWidth > 0)
                freeRects.Add(new FreeRectangle(
                    X: used.X + usedWidth,
                    Y: used.Y,
                    Width: rightWidth,
                    Length: used.Length));

            // Top rectangle (remaining width)
            if (topLength > 0)
                freeRects.Add(new FreeRectangle(
                    X: used.X,
                    Y: used.Y + usedLength,
                    Width: usedWidth,
                    Length: topLength));
        }
        else
        {
            // Top rectangle (full width)
            if (topLength > 0)
                freeRects.Add(new FreeRectangle(
                    X: used.X,
                    Y: used.Y + usedLength,
                    Width: used.Width,
                    Length: topLength));

            // Right rectangle (remaining height)
            if (rightWidth > 0)
                freeRects.Add(new FreeRectangle(
                    X: used.X + usedWidth,
                    Y: used.Y,
                    Width: rightWidth,
                    Length: usedLength));
        }
    }
}