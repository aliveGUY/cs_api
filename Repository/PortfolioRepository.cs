using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
  public class PortfolioRepository(ApplicationDBContext context) : IPortfolioRepository
  {
    private readonly ApplicationDBContext _context = context;
    public async Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
      return await _context.Portfolios.Where(u => user.Id == u.AppUserId)
      .Select(stock => new Stock
      {
        Id = stock.StockId,
        Symbol = stock.Stock.Symbol,
        CompanyName = stock.Stock.CompanyName,
        Purchase = stock.Stock.Purchase,
        LastDiv = stock.Stock.LastDiv,
        Industry = stock.Stock.Industry,
        MarketCap = stock.Stock.MarketCap
      }).ToListAsync();
    }
  }
}