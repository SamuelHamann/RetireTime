using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Interfaces;

namespace RetirementTime.Infrastructure.Repositories;

public class SubdivisionRepository(ApplicationDbContext context) : ISubdivisionRepository
{
    public async Task<List<Subdivision>> GetSubdivisionsByCountryId(int countryId)
    {
        return  await context.Subdivisions
            .Where(s => s.CountryId == countryId)
            .ToListAsync();
    }
}