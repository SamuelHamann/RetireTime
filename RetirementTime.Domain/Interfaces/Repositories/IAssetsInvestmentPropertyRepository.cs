using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IAssetsInvestmentPropertyRepository
{
    Task<List<AssetsInvestmentProperty>> GetByScenarioIdAsync(long scenarioId);
    Task<AssetsInvestmentProperty> CreateAsync(AssetsInvestmentProperty property);
    Task<bool> UpdateAsync(AssetsInvestmentProperty property);
    Task<bool> DeleteAsync(long propertyId);
}
