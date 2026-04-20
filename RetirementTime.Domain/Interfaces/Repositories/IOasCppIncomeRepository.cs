using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOasCppIncomeRepository
{
    Task<OasCppIncome?> GetByScenarioIdAsync(long scenarioId);
    Task<OasCppIncome> UpsertAsync(OasCppIncome income);
}
