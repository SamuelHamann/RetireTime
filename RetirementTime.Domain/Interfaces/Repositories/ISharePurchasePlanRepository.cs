using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface ISharePurchasePlanRepository
{
    Task<List<SharePurchasePlan>> GetByScenarioIdAsync(long scenarioId);
    Task<SharePurchasePlan> CreateAsync(SharePurchasePlan plan);
    Task<bool> UpdateAsync(SharePurchasePlan plan);
    Task<bool> DeleteAsync(long planId);
}
