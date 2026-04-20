using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IAssetsHomeRepository
{
    Task<AssetsHome?> GetByScenarioIdAsync(long scenarioId);
    Task<AssetsHome> UpsertAsync(AssetsHome home);
}
