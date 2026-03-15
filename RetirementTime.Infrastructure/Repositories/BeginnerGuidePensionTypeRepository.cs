using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class BeginnerGuidePensionTypeRepository(ApplicationDbContext context) : IBeginnerGuidePensionTypeRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<BeginnerGuidePensionType>> GetAllAsync()
    {
        return await _context.PensionTypes
            .OrderBy(pt => pt.Name)
            .ToListAsync();
    }
}

