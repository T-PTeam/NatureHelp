using Application.Interfaces.Services.Audit;
using Application.Services.Audit;
using Domain.Enums;
using Domain.Models.Audit;
using Domain.Models.Nature;
using Infrastructure.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.Application;
public class AuditTests
{
    private readonly Mock<IChangedModelLogRepository> _changedModelLogRepositoryMock;
    private readonly IChangedModelLogService _logger;

    public AuditTests()
    {
        _changedModelLogRepositoryMock = new Mock<IChangedModelLogRepository>();
        _logger = new ChangedModelLogService(_changedModelLogRepositoryMock.Object);
    }

    [Fact]
    public async Task LogDeficiencyChangesAsync_ShouldLog()
    {
        var oldDeficiency = new WaterDeficiency
        {
            Id = Guid.NewGuid(),
            Title = "Old Title",
            NitrateConcentration = 1.0
        };

        var newDeficiency = new WaterDeficiency
        {
            Id = oldDeficiency.Id,
            Title = "New Title",
            NitrateConcentration = 2.0
        };

        await _logger.LogDeficiencyChangesAsync(oldDeficiency, newDeficiency, EDeficiencyType.Soil, Guid.NewGuid());

        _changedModelLogRepositoryMock.Verify(x => x.AddAsync(It.Is<ChangedModelLog>(log =>
            log.DeficiencyType == EDeficiencyType.Soil &&
            log.DeficiencyId == newDeficiency.Id &&
            log.ChangesJson.Contains("Title") &&
            log.ChangesJson.Contains("NitrateConcentration")
        )), Times.Once);
    }

    [Fact]
    public async Task LogDeficiencyChangesAsync_ShouldNotLog()
    {
        var deficiency = new WaterDeficiency
        {
            Id = Guid.NewGuid(),
            Title = "SameTitle"
        };

        await _logger.LogDeficiencyChangesAsync(deficiency, deficiency, EDeficiencyType.Water, Guid.NewGuid());

        _changedModelLogRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ChangedModelLog>()), Times.Never);
    }
}
