using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IInvestmentPropertyRepository
{
    Task<List<InvestmentProperty>> GetByUserIdAsync(long userId);
    Task<InvestmentProperty?> GetByIdAsync(long id);
    Task<InvestmentProperty> AddAsync(InvestmentProperty property);
    Task<InvestmentProperty> UpdateAsync(InvestmentProperty property);
    Task DeleteAsync(long id);
    Task DeleteByUserIdAsync(long userId);
    Task<List<InvestmentProperty>> UpsertPropertiesAsync(long userId, List<InvestmentProperty> properties);
}

