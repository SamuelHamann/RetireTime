using RetirementTime.Domain.Entities.Dashboard;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface INetWorthHistoryRepository
{
    Task InsertBatchAsync(IEnumerable<NetWorthHistory> snapshots, CancellationToken cancellationToken);
}
