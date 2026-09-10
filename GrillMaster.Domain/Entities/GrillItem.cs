namespace GrillMaster.Domain.Entities;

/// <summary>
/// Represents a single barbecue item to be placed on the grill.
/// </summary>
public sealed class GrillItem
{
    public Guid Id { get; }
    public string Name { get; }

    /// <summary>Length of the item in centimeters. </summary>
    public int Length { get; }

    /// <summary> Width of the item in centimeters.</summary>
    public int Width { get; }

    public int Quantity { get; }

    public GrillItem(Guid id, string name, int length, int width, int quantity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        Id = id;
        Name = name;
        Length = length;
        Width = width;
        Quantity = quantity;
    }

    ///<summary> Area occupied by one unit of this item.</summary>
    public int Area => Length * Width;
}
