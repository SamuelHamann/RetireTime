using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IGenericDebtRepository
{
    Task<GenericDebt?> GetHomeMortgageAsync(long scenarioId);
    Task<List<GenericDebt>> GetAllByScenarioIdAsync(long scenarioId);
    Task<List<GenericDebt>> GetByDebtTypeIdsAsync(long scenarioId, long[] debtTypeIds);
    Task<List<Frequency>> GetFrequenciesAsync();
    Task<List<DebtType>> GetDebtTypesByIdsAsync(long[] ids);
    Task<GenericDebt> UpsertHomeMortgageAsync(long scenarioId, GenericDebt debt);
    Task<GenericDebt> CreateAsync(GenericDebt debt);
    Task<bool> UpdateAsync(GenericDebt debt);
    Task<bool> DeleteAsync(long debtId);
}
