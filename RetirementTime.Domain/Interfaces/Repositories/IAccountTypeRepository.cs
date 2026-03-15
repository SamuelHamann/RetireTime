using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IAccountTypeRepository
{
    Task<List<BeginnerGuideAccountType>> GetAllAsync();
    Task<List<BeginnerGuideAccountType>> GetByCountryIdAsync(int countryId);
    Task<BeginnerGuideAccountType?> GetByIdAsync(int id);
}

