using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IAccountTypeRepository
{
    Task<List<AccountType>> GetAllAsync();
    Task<List<AccountType>> GetByCountryIdAsync(int countryId);
    Task<AccountType?> GetByIdAsync(int id);
}

