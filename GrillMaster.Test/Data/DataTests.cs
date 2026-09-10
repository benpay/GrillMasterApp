using GrillMaster.Domain.DTOs;
using GrillMaster.Domain.Entities;

namespace GrillMaster.Test.Data;

internal class DataTests
{
    internal static MenuRequestDto BuildRequest(int itemCount) => new(
    Id: Guid.NewGuid().ToString(),
    Menu: "Menu Test",
    Items:
    [
        new GrillItemDto(
                Id: Guid.NewGuid().ToString(),
                Name: "Sausage",
                Length: 6,
                Width: 3,
                Duration: "00:08:00",
                Quantity: itemCount)
    ]);

    internal static GrillItem[] GrillItems(int itemCount)
    {
        return new GrillItem[]
        {
            new GrillItem(Guid.NewGuid(), "Sausage", 6, 3, itemCount),
            new GrillItem(Guid.NewGuid(), "Burguer", 10, 10, itemCount),
            new GrillItem(Guid.NewGuid(), "Steak", 2, 7, itemCount)
        };
    }
}


