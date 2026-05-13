using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class SharePurchasePlanRepository(ApplicationDbContext context) : ISharePurchasePlanRepository
{
    public async Task<List<SharePurchasePlan>> GetByScenarioIdAsync(long scenarioId, long timelineId)
    {
        return await context.SharePurchasePlans
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<SharePurchasePlan> CreateAsync(SharePurchasePlan plan)
    {
        plan.CreatedAt = DateTime.UtcNow;
        plan.UpdatedAt = DateTime.UtcNow;
        context.SharePurchasePlans.Add(plan);
        await context.SaveChangesAsync();
        return plan;
    }

    public async Task<bool> UpdateAsync(SharePurchasePlan plan)
    {
        var existing = await context.SharePurchasePlans.FindAsync(plan.Id);
        if (existing == null) return false;

        existing.Name = plan.Name;
        existing.PercentOfSalaryEmployee = plan.PercentOfSalaryEmployee;
        existing.PercentOfSalaryEmployer = plan.PercentOfSalaryEmployer;
        existing.UseFlatAmountInsteadOfPercent = plan.UseFlatAmountInsteadOfPercent;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long planId)
    {
        var existing = await context.SharePurchasePlans.FindAsync(planId);
        if (existing == null) return false;

        context.SharePurchasePlans.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
