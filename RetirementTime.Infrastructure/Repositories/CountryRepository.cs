using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Interfaces;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class CountryRepository(ApplicationDbContext context) : ICountryRepository
{
    public async Task<List<Country>> GetCountries()
    {
        return await context.Countries.ToListAsync();
    }
}