using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class DashboardScenarioRepository(ApplicationDbContext context) : IDashboardScenarioRepository
{
    public async Task<List<DashboardScenario>> GetAllByUserIdAsync(long userId)
    {
        return await context.DashboardScenarios
            .Where(s => s.UserId == userId)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<DashboardScenario?> GetByIdAsync(long scenarioId)
    {
        return await context.DashboardScenarios
            .FirstOrDefaultAsync(s => s.ScenarioId == scenarioId);
    }

    public async Task<long> CreateAsync(DashboardScenario scenario)
    {
        context.DashboardScenarios.Add(scenario);
        await context.SaveChangesAsync();
        return scenario.ScenarioId;
    }

    public async Task<bool> UpdateAsync(DashboardScenario scenario)
    {
        scenario.UpdatedAt = DateTime.UtcNow;
        context.DashboardScenarios.Update(scenario);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long scenarioId)
    {
        var scenario = await GetByIdAsync(scenarioId);
        if (scenario == null) return false;

        context.DashboardScenarios.Remove(scenario);
        return await context.SaveChangesAsync() > 0;
    }
}
