using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class PensionDefinedContributionRepository(ApplicationDbContext context) : IPensionDefinedContributionRepository
{
    public async Task<List<PensionDefinedContribution>> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.PensionDefinedContributions
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<PensionDefinedContribution> CreateAsync(PensionDefinedContribution plan)
    {
        plan.CreatedAt = DateTime.UtcNow;
        plan.UpdatedAt = DateTime.UtcNow;
        context.PensionDefinedContributions.Add(plan);
        await context.SaveChangesAsync();
        return plan;
    }

    public async Task<bool> UpdateAsync(PensionDefinedContribution plan)
    {
        var existing = await context.PensionDefinedContributions.FindAsync(plan.Id);
        if (existing == null) return false;

        existing.Name = plan.Name;
        existing.PercentOfSalaryEmployee = plan.PercentOfSalaryEmployee;
        existing.PercentOfSalaryEmployer = plan.PercentOfSalaryEmployer;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long planId)
    {
        var existing = await context.PensionDefinedContributions.FindAsync(planId);
        if (existing == null) return false;

        context.PensionDefinedContributions.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
