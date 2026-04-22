using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOtherPersistingIncomeRepository
{
    Task<List<OtherPersistingIncome>> GetByScenarioIdAsync(long scenarioId);
    Task<OtherPersistingIncome> CreateAsync(OtherPersistingIncome income);
    Task<bool> UpdateAsync(OtherPersistingIncome income);
    Task<bool> DeleteAsync(long incomeId);
}

