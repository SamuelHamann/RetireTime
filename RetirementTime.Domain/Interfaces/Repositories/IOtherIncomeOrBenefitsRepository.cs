using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOtherIncomeOrBenefitsRepository
{
    Task<List<OtherIncomeOrBenefits>> GetByScenarioIdAsync(long scenarioId);
    Task<OtherIncomeOrBenefits> CreateAsync(OtherIncomeOrBenefits income);
    Task<bool> UpdateAsync(OtherIncomeOrBenefits income);
    Task<bool> DeleteAsync(long incomeId);
}
