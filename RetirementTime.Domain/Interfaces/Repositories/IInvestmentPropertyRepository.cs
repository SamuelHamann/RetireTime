using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IInvestmentPropertyRepository
{
    Task<List<BeginnerGuideInvestmentProperty>> GetByUserIdAsync(long userId);
    Task<BeginnerGuideInvestmentProperty?> GetByIdAsync(long id);
    Task<BeginnerGuideInvestmentProperty> AddAsync(BeginnerGuideInvestmentProperty property);
    Task<BeginnerGuideInvestmentProperty> UpdateAsync(BeginnerGuideInvestmentProperty property);
    Task DeleteAsync(long id);
    Task DeleteByUserIdAsync(long userId);
    Task<List<BeginnerGuideInvestmentProperty>> UpsertPropertiesAsync(long userId, List<BeginnerGuideInvestmentProperty> properties);
}

