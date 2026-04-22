using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IRealEstateIncomeRepository
{
    Task<List<RealEstateIncome>> GetByScenarioIdAsync(long scenarioId);
    Task<RealEstateIncome> CreateAsync(RealEstateIncome income);
    Task<bool> UpdateAsync(RealEstateIncome income);
    Task<bool> DeleteAsync(long incomeId);
}

