using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class SelfEmploymentIncomeRepository(ApplicationDbContext context) : ISelfEmploymentIncomeRepository
{
    public async Task<List<SelfEmploymentIncome>> GetByScenarioIdAsync(long scenarioId, long timelineId)
    {
        return await context.SelfEmploymentIncomes
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<SelfEmploymentIncome> CreateAsync(SelfEmploymentIncome selfEmploymentIncome)
    {
        selfEmploymentIncome.CreatedAt = DateTime.UtcNow;
        selfEmploymentIncome.UpdatedAt = DateTime.UtcNow;
        context.SelfEmploymentIncomes.Add(selfEmploymentIncome);
        await context.SaveChangesAsync();
        return selfEmploymentIncome;
    }

    public async Task<bool> UpdateAsync(SelfEmploymentIncome selfEmploymentIncome)
    {
        var existing = await context.SelfEmploymentIncomes.FindAsync(selfEmploymentIncome.Id);
        if (existing == null) return false;

        existing.Name = selfEmploymentIncome.Name;
        existing.GrossSalary = selfEmploymentIncome.GrossSalary;
        existing.GrossSalaryFrequencyId = selfEmploymentIncome.GrossSalaryFrequencyId;
        existing.NetSalary = selfEmploymentIncome.NetSalary;
        existing.NetSalaryFrequencyId = selfEmploymentIncome.NetSalaryFrequencyId;
        existing.GrossDividends = selfEmploymentIncome.GrossDividends;
        existing.GrossDividendsFrequencyId = selfEmploymentIncome.GrossDividendsFrequencyId;
        existing.NetDividends = selfEmploymentIncome.NetDividends;
        existing.NetDividendsFrequencyId = selfEmploymentIncome.NetDividendsFrequencyId;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long selfEmploymentIncomeId)
    {
        var existing = await context.SelfEmploymentIncomes.FindAsync(selfEmploymentIncomeId);
        if (existing == null) return false;

        context.SelfEmploymentIncomes.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
