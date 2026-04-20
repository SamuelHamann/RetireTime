using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class GenericDebtRepository(ApplicationDbContext context) : IGenericDebtRepository
{
    public async Task<GenericDebt?> GetHomeMortgageAsync(long scenarioId)
    {
        return await context.GenericDebts
            .FirstOrDefaultAsync(e => e.ScenarioId == scenarioId && e.IsHomeMortgage);
    }

    public async Task<List<GenericDebt>> GetAllByScenarioIdAsync(long scenarioId)
    {
        return await context.GenericDebts
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<GenericDebt>> GetByDebtTypeIdsAsync(long scenarioId, long[] debtTypeIds)
    {
        return await context.GenericDebts
            .Where(e => e.ScenarioId == scenarioId && debtTypeIds.Contains(e.DebtTypeId) && !e.IsHomeMortgage)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Frequency>> GetFrequenciesAsync()
    {
        return await context.Frequencies.OrderBy(f => f.Id).ToListAsync();
    }

    public async Task<List<DebtType>> GetDebtTypesByIdsAsync(long[] ids)
    {
        return await context.DebtTypes
            .Where(dt => ids.Contains(dt.Id))
            .OrderBy(dt => dt.Id)
            .ToListAsync();
    }

    public async Task<GenericDebt> UpsertHomeMortgageAsync(long scenarioId, GenericDebt debt)
    {
        var existing = await context.GenericDebts
            .FirstOrDefaultAsync(e => e.ScenarioId == scenarioId && e.IsHomeMortgage);

        if (existing == null)
        {
            debt.ScenarioId = scenarioId;
            debt.IsHomeMortgage = true;
            debt.DebtTypeId = (long)DebtTypeEnum.Mortgage;
            debt.Name = string.Empty;
            debt.CreatedAt = DateTime.UtcNow;
            debt.UpdatedAt = DateTime.UtcNow;
            context.GenericDebts.Add(debt);
            await context.SaveChangesAsync();
            return debt;
        }

        existing.Balance = debt.Balance;
        existing.InterestRate = debt.InterestRate;
        existing.FrequencyId = debt.FrequencyId;
        existing.TermInYears = debt.TermInYears;
        existing.DebtAgainstAssetId = debt.DebtAgainstAssetId;
        existing.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<GenericDebt> CreateAsync(GenericDebt debt)
    {
        debt.CreatedAt = DateTime.UtcNow;
        debt.UpdatedAt = DateTime.UtcNow;
        context.GenericDebts.Add(debt);
        await context.SaveChangesAsync();
        return debt;
    }

    public async Task<bool> UpdateAsync(GenericDebt debt)
    {
        var existing = await context.GenericDebts.FindAsync(debt.Id);
        if (existing == null) return false;

        existing.Name = debt.Name;
        existing.DebtTypeId = debt.DebtTypeId;
        existing.Balance = debt.Balance;
        existing.InterestRate = debt.InterestRate;
        existing.FrequencyId = debt.FrequencyId;
        existing.TermInYears = debt.TermInYears;
        existing.DebtAgainstAssetId = debt.DebtAgainstAssetId;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long debtId)
    {
        var existing = await context.GenericDebts.FindAsync(debtId);
        if (existing == null) return false;

        context.GenericDebts.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
