using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class OtherPersistingIncomeRepository(ApplicationDbContext context) : IOtherPersistingIncomeRepository
{
    public async Task<List<OtherPersistingIncome>> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.OtherPersistingIncomes
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<OtherPersistingIncome> CreateAsync(OtherPersistingIncome income)
    {
        income.CreatedAt = DateTime.UtcNow;
        income.UpdatedAt = DateTime.UtcNow;
        context.OtherPersistingIncomes.Add(income);
        await context.SaveChangesAsync();
        return income;
    }

    public async Task<bool> UpdateAsync(OtherPersistingIncome income)
    {
        var existing = await context.OtherPersistingIncomes.FindAsync(income.Id);
        if (existing == null) return false;

        existing.Name = income.Name;
        existing.Amount = income.Amount;
        existing.FrequencyId = income.FrequencyId;
        existing.Taxable = income.Taxable;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long incomeId)
    {
        var existing = await context.OtherPersistingIncomes.FindAsync(incomeId);
        if (existing == null) return false;

        context.OtherPersistingIncomes.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}

