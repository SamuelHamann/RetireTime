using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface ISubdivisionRepository
{
    public Task<List<Subdivision>> GetSubdivisionsByCountryId(int countryId);
}