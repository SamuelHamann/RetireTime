using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOasCppIncomeRepository
{
    Task<OasCppIncome?> GetByScenarioIdAsync(long scenarioId, long timelineId);
    Task<OasCppIncome> UpsertAsync(OasCppIncome income);
}
