using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Domain.Interfaces;

public interface ICountryRepository
{
    public Task<List<Country>> GetCountries();
}