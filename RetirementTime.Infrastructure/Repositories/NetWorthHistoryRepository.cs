using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class NetWorthHistoryRepository(ApplicationDbContext context) : INetWorthHistoryRepository
{
    public async Task InsertBatchAsync(IEnumerable<NetWorthHistory> snapshots, CancellationToken cancellationToken)
    {
        context.NetWorthHistories.AddRange(snapshots);
        await context.SaveChangesAsync(cancellationToken);
    }
}
