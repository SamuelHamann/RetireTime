using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class AccountTypeRepository : IAccountTypeRepository
{
    private readonly ApplicationDbContext _context;

    public AccountTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<BeginnerGuideAccountType>> GetAllAsync()
    {
        return await _context.AccountTypes.ToListAsync();
    }

    public async Task<List<BeginnerGuideAccountType>> GetByCountryIdAsync(int countryId)
    {
        return await _context.AccountTypes
            .Where(a => a.CountryId == countryId)
            .ToListAsync();
    }

    public async Task<BeginnerGuideAccountType?> GetByIdAsync(int id)
    {
        return await _context.AccountTypes.FindAsync(id);
    }
}

