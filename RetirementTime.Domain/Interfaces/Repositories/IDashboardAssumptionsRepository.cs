using RetirementTime.Domain.Entities.Dashboard;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IDashboardAssumptionsRepository
{
    Task<DashboardAssumptions?> GetByScenarioIdAsync(long scenarioId);
    Task<long> CreateAsync(DashboardAssumptions assumptions);
    Task<bool> UpdateAsync(DashboardAssumptions assumptions);
}

