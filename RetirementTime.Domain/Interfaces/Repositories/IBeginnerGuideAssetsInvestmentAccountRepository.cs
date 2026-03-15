using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IBeginnerGuideAssetsInvestmentAccountRepository
{
    Task<BeginnerGuideAssetsInvestmentAccount?> GetByIdAsync(int id);
    Task<List<BeginnerGuideAssetsInvestmentAccount>> GetByUserIdAsync(long userId);
    Task<BeginnerGuideAssetsInvestmentAccount> AddAsync(BeginnerGuideAssetsInvestmentAccount account);
    Task<List<BeginnerGuideAssetsInvestmentAccount>> AddRangeAsync(List<BeginnerGuideAssetsInvestmentAccount> accounts);
    Task<BeginnerGuideAssetsInvestmentAccount> UpdateAsync(BeginnerGuideAssetsInvestmentAccount account);
    Task DeleteAsync(int id);
    Task DeleteByUserIdAsync(long userId);
    Task<(List<BeginnerGuideAssetsInvestmentAccount> SavedAccounts, List<int> AccountIds)> UpsertAccountsAsync<TStockDto>(
        long userId,
        List<BeginnerGuideAssetsInvestmentAccount> accounts,
        Dictionary<int, List<TStockDto>> accountStocksMap,
        IBeginnerGuideAssetsStockDataRepository stockDataRepository) where TStockDto : class;
}

