using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IRetirementSpendingRepository
{
    Task<List<RetirementSpending>> GetByScenarioIdAsync(long scenarioId);
    Task<RetirementSpending?> GetByIdAsync(long id);
    Task<RetirementSpending> CreateAsync(RetirementSpending entity);
    Task UpdateAsync(RetirementSpending entity);
    Task DeleteAsync(long id);
}

