using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IBeginnerGuideAssetsStockDataRepository
{
    Task<BeginnerGuideAssetsStockData?> GetByIdAsync(int id);
    Task<List<BeginnerGuideAssetsStockData>> GetByInvestmentAccountIdAsync(int investmentAccountId);
    Task<BeginnerGuideAssetsStockData> AddAsync(BeginnerGuideAssetsStockData stock);
    Task<List<BeginnerGuideAssetsStockData>> AddRangeAsync(List<BeginnerGuideAssetsStockData> stocks);
    Task<BeginnerGuideAssetsStockData> UpdateAsync(BeginnerGuideAssetsStockData stock);
    Task DeleteAsync(int id);
    Task DeleteByInvestmentAccountIdAsync(int investmentAccountId);
}

