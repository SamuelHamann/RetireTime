using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IPensionDefinedContributionRepository
{
    Task<List<PensionDefinedContribution>> GetByScenarioIdAsync(long scenarioId, long timelineId);
    Task<PensionDefinedContribution> CreateAsync(PensionDefinedContribution plan);
    Task<bool> UpdateAsync(PensionDefinedContribution plan);
    Task<bool> DeleteAsync(long planId);
}
