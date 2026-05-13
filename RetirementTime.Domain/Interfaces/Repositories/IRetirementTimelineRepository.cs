using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IRetirementTimelineRepository
{
    Task<List<RetirementTimeline>> GetByScenarioIdAsync(long scenarioId);
    Task<RetirementTimeline?> GetByIdAsync(long id);
    Task<RetirementTimeline> CreateAsync(RetirementTimeline entity);
    Task UpdateAsync(RetirementTimeline entity);
    Task DeleteAsync(long id);
    Task CloneIncomeFromTimelineAsync(long scenarioId, long sourceTimelineId, long targetTimelineId);
}

