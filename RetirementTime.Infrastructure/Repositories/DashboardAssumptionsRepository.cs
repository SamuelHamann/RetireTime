using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class DashboardAssumptionsRepository(ApplicationDbContext context) : IDashboardAssumptionsRepository
{
    public async Task<DashboardAssumptions?> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.DashboardAssumptions
            .FirstOrDefaultAsync(a => a.ScenarioId == scenarioId);
    }

    public async Task<long> CreateAsync(DashboardAssumptions assumptions)
    {
        context.DashboardAssumptions.Add(assumptions);
        await context.SaveChangesAsync();
        return assumptions.Id;
    }

    public async Task<bool> UpdateAsync(DashboardAssumptions assumptions)
    {
        assumptions.UpdatedAt = DateTime.UtcNow;
        context.DashboardAssumptions.Update(assumptions);
        return await context.SaveChangesAsync() > 0;
    }
}

