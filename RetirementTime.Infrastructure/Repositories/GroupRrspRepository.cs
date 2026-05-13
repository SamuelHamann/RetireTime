using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class GroupRrspRepository(ApplicationDbContext context) : IGroupRrspRepository
{
    public async Task<List<GroupRrsp>> GetByScenarioIdAsync(long scenarioId, long timelineId)
    {
        return await context.GroupRrsps
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<GroupRrsp> CreateAsync(GroupRrsp plan)
    {
        plan.CreatedAt = DateTime.UtcNow;
        plan.UpdatedAt = DateTime.UtcNow;
        context.GroupRrsps.Add(plan);
        await context.SaveChangesAsync();
        return plan;
    }

    public async Task<bool> UpdateAsync(GroupRrsp plan)
    {
        var existing = await context.GroupRrsps.FindAsync(plan.Id);
        if (existing == null) return false;

        existing.Name = plan.Name;
        existing.PercentOfSalaryEmployee = plan.PercentOfSalaryEmployee;
        existing.PercentOfSalaryEmployer = plan.PercentOfSalaryEmployer;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long planId)
    {
        var existing = await context.GroupRrsps.FindAsync(planId);
        if (existing == null) return false;

        context.GroupRrsps.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
