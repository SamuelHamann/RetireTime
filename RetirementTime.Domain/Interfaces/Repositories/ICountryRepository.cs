using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface ICountryRepository
{
    public Task<List<Country>> GetCountries();
}