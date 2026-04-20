using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface ISelfEmploymentIncomeRepository
{
    Task<List<SelfEmploymentIncome>> GetByScenarioIdAsync(long scenarioId);
    Task<SelfEmploymentIncome> CreateAsync(SelfEmploymentIncome selfEmploymentIncome);
    Task<bool> UpdateAsync(SelfEmploymentIncome selfEmploymentIncome);
    Task<bool> DeleteAsync(long selfEmploymentIncomeId);
}
