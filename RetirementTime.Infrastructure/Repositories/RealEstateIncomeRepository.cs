using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class RealEstateIncomeRepository(ApplicationDbContext context) : IRealEstateIncomeRepository
{
    public async Task<List<RealEstateIncome>> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.RealEstateIncomes
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<RealEstateIncome> CreateAsync(RealEstateIncome income)
    {
        income.CreatedAt = DateTime.UtcNow;
        income.UpdatedAt = DateTime.UtcNow;
        context.RealEstateIncomes.Add(income);
        await context.SaveChangesAsync();
        return income;
    }

    public async Task<bool> UpdateAsync(RealEstateIncome income)
    {
        var existing = await context.RealEstateIncomes.FindAsync(income.Id);
        if (existing == null) return false;

        existing.PropertyName = income.PropertyName;
        existing.Amount = income.Amount;
        existing.FrequencyId = income.FrequencyId;
        existing.InvestmentPropertyId = income.InvestmentPropertyId;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long incomeId)
    {
        var existing = await context.RealEstateIncomes.FindAsync(incomeId);
        if (existing == null) return false;

        context.RealEstateIncomes.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}

