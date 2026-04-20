namespace RetirementTime.Domain.Interfaces.Services;

public interface INetWorthSnapshotService
{
    Task ProcessScenarioBatchAsync(IEnumerable<long> scenarioIds, CancellationToken cancellationToken);
}
