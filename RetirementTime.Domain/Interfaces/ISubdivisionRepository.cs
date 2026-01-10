using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Domain.Interfaces;

public interface ISubdivisionRepository
{
    public Task<List<Subdivision>> GetSubdivisionsByCountryId(int countryId);
}