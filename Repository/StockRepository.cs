using Microsoft.EntityFrameworkCore;

using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
  public class StockRepository : IStockRepository
  {
    private readonly ApplicationDBContext _context;
    public StockRepository(ApplicationDBContext context)
    {
      _context = context;
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
      await _context.Stocks.AddAsync(stockModel);
      await _context.SaveChangesAsync();
      return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
      var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => id == x.Id);
      if (null == stockModel)
      {
        return null;
      }

      _context.Stocks.Remove(stockModel);
      return stockModel;
    }

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
      var stock = _context.Stocks.Include(c => c.Comments).AsQueryable();

      if (!string.IsNullOrWhiteSpace(query.CompanyName))
        stock = stock.Where(s => s.CompanyName.Contains(query.CompanyName));

      if (!string.IsNullOrWhiteSpace(query.Symbol))
        stock = stock.Where(s => s.Symbol.Contains(query.Symbol));

      if (!string.IsNullOrWhiteSpace(query.SortBy))
      {
        if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
          stock = query.IsDescending ? stock.OrderByDescending(s => s.Symbol) : stock.OrderBy(s => s.Symbol);
        if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
          stock = query.IsDescending ? stock.OrderByDescending(s => s.CompanyName) : stock.OrderBy(s => s.CompanyName);
        if (query.SortBy.Equals("Industry", StringComparison.OrdinalIgnoreCase))
          stock = query.IsDescending ? stock.OrderByDescending(s => s.Industry) : stock.OrderBy(s => s.Industry);
      }

      var skipNumber = (query.PageNumber - 1) * query.PageSize;


      return await stock.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
      return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => id == i.Id);
    }

    public Task<bool> StockExists(int id)
    {
      return _context.Stocks.AnyAsync(s => id == s.Id);
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
    {
      var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => id == x.Id);

      if (null == existingStock)
      {
        return null;
      }

      existingStock.Symbol = stockDto.Symbol;
      existingStock.CompanyName = stockDto.CompanyName;
      existingStock.Purchase = stockDto.Purchase;
      existingStock.LastDiv = stockDto.LastDiv;
      existingStock.Industry = stockDto.Industry;
      existingStock.MarketCap = stockDto.MarketCap;

      await _context.SaveChangesAsync();
      return existingStock;
    }
  }
}