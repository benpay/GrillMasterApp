using GrillMaster.Domain.Entities;
using GrillMaster.Infrastructure.Packing;
using GrillMaster.Test.Data;

namespace GrillMaster.Tests.Packing;

public sealed class GuillotineBinPackerTests
{
    private readonly GuillotinePacker _packer = new();

    // Single food item smaller than grill
    [Fact]
    public void Pack_SingleItemFitsInOneSession_ReturnsSingleSession()
    {
        var items = new[] { new GrillItem(Guid.NewGuid(), "Sausage", 6, 3, 1) };

        var sessions = _packer.Pack(items);

        Assert.Single(sessions);
        Assert.Single(sessions[0].PlacedItems);
    }

    // 6 burgers of 10x10 (100cm area) — grill is 20x30 (600cm area)
    // 6 burguers are 600cm area so max 6 per session (6 burguer = 1 session)
    [Fact]
    public void Pack_ItemsWithGrillCapacity_CreatesMultipleSessions()
    {
        var items = Enumerable.Range(0, 6)
            .Select(_ => new GrillItem(Guid.NewGuid(), "Burger", 10, 10, 1))
            .ToList();

        var sessions = _packer.Pack(items);

        Assert.True(sessions.Count == 1);
    }

    // 7 burguers are 700cm area so it need 2 sessions
    // Session 1 = 6 burguer // Session 2 = 1 burguer
    [Fact]
    public void Pack_ItemsExceedGrillCapacity_CreatesMultipleSessions()
    {
        var items = Enumerable.Range(0, 7)
            .Select(_ => new GrillItem(Guid.NewGuid(), "Burger", 10, 10, 1))
            .ToList();

        var sessions = _packer.Pack(items);

        Assert.True(sessions.Count == 2);
    }

    // Different sizes for the grill in one session
    [Fact]
    public void Pack_DiferentItemsFitsInOneSession_ReturnsSingleSession()
    {
        var items = DataTests.GrillItems(1);

        var sessions = _packer.Pack(items);

        Assert.Single(sessions);
        Assert.True(sessions[0].PlacedItems.Count == 3);
    }

    // No items into a pack should return empty array with ArgumentNullException
    [Fact]
    public void Pack_EmptyInput_ReturnsNoSessions()
    {
        var sessions = _packer.Pack([]);

        Assert.Empty(sessions);
        ArgumentNullException.Equals(sessions, null);
    }

    // 
    [Fact]
    public void Pack_AllItemsArePlaced_NoneAreLost()
    {
        var items = Enumerable.Range(0, 10)
            .Select(_ => new GrillItem(Guid.NewGuid(), "Sausage", 6, 3, 1))
            .ToList();

        var sessions = _packer.Pack(items);
        var totalPlaced = sessions.Sum(s => s.PlacedItems.Count);

        Assert.Equal(10, totalPlaced);
    }

    [Fact]
    public void Pack_ItemsDoNotOverlap_WithinSameSession()
    {
        var items = Enumerable.Range(0, 5)
            .Select(_ => new GrillItem(Guid.NewGuid(), "Burger", 10, 10, 1))
            .ToList();

        var sessions = _packer.Pack(items);

        foreach (var session in sessions)
            AssertNoOverlaps(session.PlacedItems);
    }

    private static void AssertNoOverlaps(IReadOnlyList<PlacedItem> items)
    {
        for (int i = 0; i < items.Count; i++)
            for (int j = i + 1; j < items.Count; j++)
            {
                var a = items[i];
                var b = items[j];

                bool overlapsX = a.X < b.X + b.Width && a.X + a.Width > b.X;
                bool overlapsY = a.Y < b.Y + b.Length && a.Y + a.Length > b.Y;

                Assert.False(overlapsX && overlapsY,
                    $"Items '{a.Name}' and '{b.Name}' overlap.");
            }
    }
}