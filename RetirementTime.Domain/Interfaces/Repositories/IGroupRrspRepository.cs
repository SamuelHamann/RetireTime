using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IGroupRrspRepository
{
    Task<List<GroupRrsp>> GetByScenarioIdAsync(long scenarioId);
    Task<GroupRrsp> CreateAsync(GroupRrsp plan);
    Task<bool> UpdateAsync(GroupRrsp plan);
    Task<bool> DeleteAsync(long planId);
}
