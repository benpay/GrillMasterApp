using GrillMaster.Domain.DTOs;
using GrillMaster.Application.UseCases;
using GrillMaster.Domain.Abstractions;
using GrillMaster.Domain.Entities;
using GrillMaster.Test.Data;
using NSubstitute;

namespace GrillMaster.Tests.UseCases;

public sealed class PlanGrillSessionsUseCaseTests
{
    private readonly IBinPackingService _packingService = Substitute.For<IBinPackingService>();
    private readonly PlanGrillSessionsUseCase _useCase;

    public PlanGrillSessionsUseCaseTests()
    {
        _useCase = new PlanGrillSessionsUseCase(_packingService);
    }

    [Fact]
    public void Execute_ValidRequest_ReturnsPlanWithCorrectMenu()
    {
        _packingService.Pack(Arg.Any<IEnumerable<GrillItem>>())
            .Returns([new GrillSession(1, [])]);

        MenuRequestDto request = DataTests.BuildRequest(itemCount: 1);
        var result = _useCase.Execute(request);

        Assert.Equal(request.Menu, result.Menu);
        Assert.Equal(1, result.TotalSessions);
    }

    [Fact]
    public void Execute_QuantityTwo_ExpandsToTwoItems()
    {
        IEnumerable<GrillItem>? capturedItems = null;

        _packingService.Pack(Arg.Do<IEnumerable<GrillItem>>(i => capturedItems = i))
            .Returns([new GrillSession(1, [])]);

        MenuRequestDto request = DataTests.BuildRequest(itemCount: 2);
        _useCase.Execute(request);

        Assert.Equal(2, capturedItems?.Count());
    }
}