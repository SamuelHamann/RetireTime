using RetirementTime.Domain.Entities.Dashboard;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IDashboardScenarioRepository
{
    Task<List<DashboardScenario>> GetAllByUserIdAsync(long userId);
    Task<DashboardScenario?> GetByIdAsync(long scenarioId);
    Task<long> CreateAsync(DashboardScenario scenario);
    Task<bool> UpdateAsync(DashboardScenario scenario);
    Task<bool> DeleteAsync(long scenarioId);
}
