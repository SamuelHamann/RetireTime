using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class BeginnerGuideAssetsStockDataRepository : IBeginnerGuideAssetsStockDataRepository
{
    private readonly ApplicationDbContext _context;

    public BeginnerGuideAssetsStockDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BeginnerGuideAssetsStockData?> GetByIdAsync(int id)
    {
        return await _context.InvestmentStocks.FindAsync(id);
    }

    public async Task<List<BeginnerGuideAssetsStockData>> GetByInvestmentAccountIdAsync(int investmentAccountId)
    {
        return await _context.InvestmentStocks
            .Where(s => s.InvestmentAccountId == investmentAccountId)
            .ToListAsync();
    }

    public async Task<BeginnerGuideAssetsStockData> AddAsync(BeginnerGuideAssetsStockData stock)
    {
        await _context.InvestmentStocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        
        return stock;
    }

    public async Task<List<BeginnerGuideAssetsStockData>> AddRangeAsync(List<BeginnerGuideAssetsStockData> stocks)
    {
        await _context.InvestmentStocks.AddRangeAsync(stocks);
        await _context.SaveChangesAsync();
        
        return stocks;
    }

    public async Task<BeginnerGuideAssetsStockData> UpdateAsync(BeginnerGuideAssetsStockData stock)
    {
        _context.InvestmentStocks.Update(stock);
        await _context.SaveChangesAsync();
        
        return stock;
    }

    public async Task DeleteAsync(int id)
    {
        var stock = await _context.InvestmentStocks.FindAsync(id);
        if (stock != null)
        {
            _context.InvestmentStocks.Remove(stock);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByInvestmentAccountIdAsync(int investmentAccountId)
    {
        var stocks = await _context.InvestmentStocks
            .Where(s => s.InvestmentAccountId == investmentAccountId)
            .ToListAsync();
        
        _context.InvestmentStocks.RemoveRange(stocks);
        await _context.SaveChangesAsync();
    }
}

