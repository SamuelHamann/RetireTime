using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class AssetTypeRepository(ApplicationDbContext context) : IAssetTypeRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<AssetType>> GetAllAsync()
    {
        return await _context.AssetTypes
            .OrderBy(at => at.Name)
            .ToListAsync();
    }

    public async Task<AssetType?> GetByIdAsync(int id)
    {
        return await _context.AssetTypes.FindAsync(id);
    }
}

