using api.Extentions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  [Route("api/portfolio")]
  [ApiController]
  public class PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo) : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IStockRepository _stockRepo = stockRepo;
    private readonly IPortfolioRepository _portfolioRepo = portfolioRepo;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
      var username = User.GetUserName();
      var appUser = await _userManager.FindByNameAsync(username);
      var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
      return Ok(userPortfolio);
    }
  }
}