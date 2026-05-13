using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IDefinedProfitSharingRepository
{
    Task<List<DefinedProfitSharing>> GetByScenarioIdAsync(long scenarioId, long timelineId);
    Task<DefinedProfitSharing> CreateAsync(DefinedProfitSharing plan);
    Task<bool> UpdateAsync(DefinedProfitSharing plan);
    Task<bool> DeleteAsync(long planId);
}
