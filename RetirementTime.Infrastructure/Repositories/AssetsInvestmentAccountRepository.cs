using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class AssetsInvestmentAccountRepository(ApplicationDbContext context) : IAssetsInvestmentAccountRepository
{
    public async Task<List<AssetsInvestmentAccount>> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.AssetsInvestmentAccounts
            .Include(e => e.Holdings)
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AccountType>> GetAccountTypesAsync()
    {
        return await context.Set<AccountType>().OrderBy(e => e.Id).ToListAsync();
    }

    public async Task<AssetsInvestmentAccount> CreateAsync(AssetsInvestmentAccount account)
    {
        account.CreatedAt = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;
        context.AssetsInvestmentAccounts.Add(account);
        await context.SaveChangesAsync();
        return account;
    }

    public async Task<bool> UpdateAsync(AssetsInvestmentAccount account)
    {
        var existing = await context.AssetsInvestmentAccounts.FindAsync(account.Id);
        if (existing == null) return false;

        existing.AccountName = account.AccountName;
        existing.AccountTypeId = account.AccountTypeId;
        existing.AdjustedCostBasis = account.AdjustedCostBasis;
        existing.CurrentTotalValue = account.CurrentTotalValue;
        existing.UseIndividualHoldings = account.UseIndividualHoldings;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long accountId)
    {
        var existing = await context.AssetsInvestmentAccounts.FindAsync(accountId);
        if (existing == null) return false;

        context.AssetsInvestmentAccounts.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<AssetsHolding> CreateHoldingAsync(AssetsHolding holding)
    {
        holding.CreatedAt = DateTime.UtcNow;
        holding.UpdatedAt = DateTime.UtcNow;
        context.AssetsHoldings.Add(holding);
        await context.SaveChangesAsync();
        return holding;
    }

    public async Task<bool> UpdateHoldingAsync(AssetsHolding holding)
    {
        var existing = await context.AssetsHoldings.FindAsync(holding.Id);
        if (existing == null) return false;

        existing.AssetName = holding.AssetName;
        existing.IsPubliclyTraded = holding.IsPubliclyTraded;
        existing.CurrentValue = holding.CurrentValue;
        existing.TickerSymbol = holding.TickerSymbol;
        existing.AdjustedCostBasis = holding.AdjustedCostBasis;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteHoldingAsync(long holdingId)
    {
        var existing = await context.AssetsHoldings.FindAsync(holdingId);
        if (existing == null) return false;

        context.AssetsHoldings.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
