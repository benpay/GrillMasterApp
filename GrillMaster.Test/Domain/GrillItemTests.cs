using GrillMaster.Domain.Entities;

namespace GrillMaster.Tests.Domain;

public sealed class GrillItemTests
{
    [Fact]
    public void Constructor_ValidParameters_CreatesItem()
    {
        var item = new GrillItem(Guid.NewGuid(), "Sausage", 6, 3, 1);

        Assert.Equal("Sausage", item.Name);
        Assert.Equal(6, item.Length);
        Assert.Equal(3, item.Width);
        Assert.Equal(18, item.Area);
    }

    [Theory]
    [InlineData(0, 3)]
    [InlineData(6, 0)]
    [InlineData(0, 0)]
    [InlineData(-1, 3)]
    [InlineData(2, -4)]
    [InlineData(-1, -2)]
    public void Constructor_InvalidDimensions_Throws(int length, int width)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GrillItem(Guid.NewGuid(), "Sausage", length, width, 1));
    }

    [Fact]
    public void Constructor_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new GrillItem(Guid.NewGuid(), "", 6, 3, 1));
    }
}