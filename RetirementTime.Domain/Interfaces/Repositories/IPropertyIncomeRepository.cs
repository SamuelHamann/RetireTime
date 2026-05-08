using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IPropertyIncomeRepository
{
    Task<List<PropertyIncome>> GetByScenarioIdAsync(long scenarioId, long timelineId);
    Task<PropertyIncome> CreateAsync(PropertyIncome income);
    Task<bool> UpdateAsync(PropertyIncome income);
    Task<bool> DeleteAsync(long id);
}

