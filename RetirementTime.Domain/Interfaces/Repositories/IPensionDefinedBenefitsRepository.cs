using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IPensionDefinedBenefitsRepository
{
    Task<List<PensionDefinedBenefits>> GetByScenarioIdAsync(long scenarioId, long timelineId);
    Task<PensionDefinedBenefits> CreateAsync(PensionDefinedBenefits pension);
    Task<bool> UpdateAsync(PensionDefinedBenefits pension);
    Task<bool> DeleteAsync(long pensionId);
}
