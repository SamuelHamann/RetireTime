using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Interfaces.Repositories;
using RetirementTime.Domain.Interfaces.Services;

namespace RetirementTime.Infrastructure.Services;

public class NetWorthSnapshotService(
    ApplicationDbContext context,
    INetWorthCalculationService calculationService,
    INetWorthHistoryRepository netWorthHistoryRepository) : INetWorthSnapshotService
{
    public async Task ProcessScenarioBatchAsync(IEnumerable<long> scenarioIds, CancellationToken cancellationToken)
    {
        var idList = scenarioIds.ToList();

        var homes = await context.AssetsHomes
            .Where(a => idList.Contains(a.ScenarioId))
            .ToListAsync(cancellationToken);

        var investmentProperties = await context.AssetsInvestmentProperties
            .Where(a => idList.Contains(a.ScenarioId))
            .ToListAsync(cancellationToken);

        var investmentAccounts = await context.AssetsInvestmentAccounts
            .Include(a => a.Holdings)
            .Where(a => idList.Contains(a.ScenarioId))
            .ToListAsync(cancellationToken);

        var physicalAssets = await context.AssetsPhysicalAssets
            .Where(a => idList.Contains(a.ScenarioId))
            .ToListAsync(cancellationToken);

        var debts = await context.GenericDebts
            .Include(d => d.DebtType)
            .Where(d => idList.Contains(d.ScenarioId))
            .ToListAsync(cancellationToken);

        var snapshots = idList.Select(scenarioId => calculationService.Calculate(
            scenarioId,
            homes.FirstOrDefault(h => h.ScenarioId == scenarioId),
            investmentProperties.Where(p => p.ScenarioId == scenarioId),
            investmentAccounts.Where(a => a.ScenarioId == scenarioId),
            physicalAssets.Where(a => a.ScenarioId == scenarioId),
            debts.Where(d => d.ScenarioId == scenarioId)
        ));

        await netWorthHistoryRepository.InsertBatchAsync(snapshots, cancellationToken);
    }
}
