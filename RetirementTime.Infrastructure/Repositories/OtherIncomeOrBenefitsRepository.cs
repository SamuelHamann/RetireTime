using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class OtherIncomeOrBenefitsRepository(ApplicationDbContext context) : IOtherIncomeOrBenefitsRepository
{
    public async Task<List<OtherIncomeOrBenefits>> GetByScenarioIdAsync(long scenarioId, long timelineId)
    {
        return await context.OtherIncomeOrBenefits
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<OtherIncomeOrBenefits> CreateAsync(OtherIncomeOrBenefits income)
    {
        income.CreatedAt = DateTime.UtcNow;
        income.UpdatedAt = DateTime.UtcNow;
        context.OtherIncomeOrBenefits.Add(income);
        await context.SaveChangesAsync();
        return income;
    }

    public async Task<bool> UpdateAsync(OtherIncomeOrBenefits income)
    {
        var existing = await context.OtherIncomeOrBenefits.FindAsync(income.Id);
        if (existing == null) return false;

        existing.Name = income.Name;
        existing.Amount = income.Amount;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long incomeId)
    {
        var existing = await context.OtherIncomeOrBenefits.FindAsync(incomeId);
        if (existing == null) return false;

        context.OtherIncomeOrBenefits.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
