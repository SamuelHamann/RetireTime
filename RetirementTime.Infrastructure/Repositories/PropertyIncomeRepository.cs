using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class PropertyIncomeRepository(ApplicationDbContext context) : IPropertyIncomeRepository
{
    public async Task<List<PropertyIncome>> GetByScenarioIdAsync(long scenarioId, long timelineId)
    {
        return await context.PropertyIncomes
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<PropertyIncome> CreateAsync(PropertyIncome income)
    {
        income.CreatedAt = DateTime.UtcNow;
        income.UpdatedAt = DateTime.UtcNow;
        context.PropertyIncomes.Add(income);
        await context.SaveChangesAsync();
        return income;
    }

    public async Task<bool> UpdateAsync(PropertyIncome income)
    {
        var existing = await context.PropertyIncomes.FindAsync(income.Id);
        if (existing is null) return false;

        existing.Name        = income.Name;
        existing.Amount      = income.Amount;
        existing.FrequencyId = income.FrequencyId;
        existing.UpdatedAt   = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existing = await context.PropertyIncomes.FindAsync(id);
        if (existing is null) return false;

        context.PropertyIncomes.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}

