using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class OtherAssetRepository(ApplicationDbContext context) : IOtherAssetRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<BeginnerGuideOtherAsset>> GetByUserIdAsync(long userId)
    {
        return await _context.OtherAssets
            .Include(a => a.AssetType)
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<BeginnerGuideOtherAsset?> GetByIdAsync(int id)
    {
        return await _context.OtherAssets
            .Include(a => a.AssetType)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<BeginnerGuideOtherAsset> AddAsync(BeginnerGuideOtherAsset asset)
    {
        asset.CreatedAt = DateTime.UtcNow;
        asset.UpdatedAt = DateTime.UtcNow;

        await _context.OtherAssets.AddAsync(asset);
        await _context.SaveChangesAsync();
        
        return asset;
    }

    public async Task<BeginnerGuideOtherAsset> UpdateAsync(BeginnerGuideOtherAsset asset)
    {
        asset.UpdatedAt = DateTime.UtcNow;

        _context.OtherAssets.Update(asset);
        await _context.SaveChangesAsync();
        
        return asset;
    }

    public async Task DeleteAsync(int id)
    {
        var asset = await _context.OtherAssets.FindAsync(id);
        if (asset != null)
        {
            _context.OtherAssets.Remove(asset);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var assets = await _context.OtherAssets
            .Where(a => a.UserId == userId)
            .ToListAsync();

        _context.OtherAssets.RemoveRange(assets);
        await _context.SaveChangesAsync();
    }

    public async Task<(List<BeginnerGuideOtherAsset> SavedAssets, List<int> AssetIds)> UpsertAssetsAsync(
        long userId,
        List<BeginnerGuideOtherAsset> assets)
    {
        // Use a transaction to ensure atomicity
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // 1. Delete existing assets for this user
            var existingAssets = await _context.OtherAssets
                .Where(a => a.UserId == userId)
                .ToListAsync();

            if (existingAssets.Any())
            {
                _context.OtherAssets.RemoveRange(existingAssets);
                await _context.SaveChangesAsync();
            }

            // 2. Insert all new assets in bulk
            var now = DateTime.UtcNow;
            foreach (var asset in assets)
            {
                asset.CreatedAt = now;
                asset.UpdatedAt = now;
            }
            
            await _context.OtherAssets.AddRangeAsync(assets);
            await _context.SaveChangesAsync();
            
            var assetIds = assets.Select(a => a.Id).ToList();

            // Commit the transaction
            await transaction.CommitAsync();

            return (assets, assetIds);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

