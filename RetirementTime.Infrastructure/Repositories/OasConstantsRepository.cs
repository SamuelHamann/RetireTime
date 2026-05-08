using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RetirementTime.Infrastructure.Repositories;

public class OasConstantsRepository(ApplicationDbContext context) : IOasConstantsRepository
{
    public async Task<OasConstants?> GetLatestAsync()
        => await context.Set<OasConstants>()
            .OrderByDescending(o => o.Id)
            .FirstOrDefaultAsync();
}

