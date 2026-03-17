using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class FrequencyRepository(ApplicationDbContext context) : IFrequencyRepository
{
    public async Task<List<Frequency>> GetFrequencies()
    {
        return await context.Frequencies.OrderBy(f => f.FrequencyPerYear).ToListAsync();
    }

    public async Task<Frequency?> GetById(int id)
    {
        return await context.Frequencies.FirstOrDefaultAsync(f => f.Id == id);
    }
}

