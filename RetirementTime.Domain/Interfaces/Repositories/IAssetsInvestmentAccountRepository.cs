using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IAssetsInvestmentAccountRepository
{
    Task<List<AssetsInvestmentAccount>> GetByScenarioIdAsync(long scenarioId);
    Task<List<AccountType>> GetAccountTypesAsync();
    Task<AssetsInvestmentAccount> CreateAsync(AssetsInvestmentAccount account);
    Task<bool> UpdateAsync(AssetsInvestmentAccount account);
    Task<bool> DeleteAsync(long accountId);

    Task<AssetsHolding> CreateHoldingAsync(AssetsHolding holding);
    Task<bool> UpdateHoldingAsync(AssetsHolding holding);
    Task<bool> DeleteHoldingAsync(long holdingId);
}
