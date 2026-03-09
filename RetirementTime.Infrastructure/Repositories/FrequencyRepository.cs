using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class FrequencyRepository(ApplicationDbContext context) : IFrequencyRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Frequency>> GetAllAsync()
    {
        return await _context.Frequencies
            .OrderBy(f => f.FrequencyPerYear)
            .ToListAsync();
    }

    public async Task<Frequency?> GetByIdAsync(int id)
    {
        return await _context.Frequencies
            .FirstOrDefaultAsync(f => f.Id == id);
    }
}
