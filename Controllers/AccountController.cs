using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
  [Route("api/account")]
  [ApiController]
  public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager) : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly SignInManager<AppUser> _signInManager = signInManager;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var user = await _userManager.Users.FirstOrDefaultAsync(x => loginDto.Username.ToLower() == x.UserName);

      if (null == user)
        return Unauthorized("Invalid username");

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

      if (!result.Succeeded)
        return Unauthorized("Username not found/or password incorrect");

      return Ok(new NewUserDto
      {
        UserName = user.UserName,
        Email = user.Email,
        Token = _tokenService.CreateToken(user)
      });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
      try
      {
        if (!ModelState.IsValid)
          return BadRequest(ModelState);

        var appUser = new AppUser
        {
          UserName = registerDto.Username,
          Email = registerDto.Email
        };

        var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

        if (!createdUser.Succeeded)
          return StatusCode(500, createdUser.Errors);

        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

        if (!roleResult.Succeeded)
          return StatusCode(500, roleResult.Errors);

        return Ok(
          new NewUserDto
          {
            UserName = appUser.UserName,
            Email = appUser.Email,
            Token = _tokenService.CreateToken(appUser),
          }
        );
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }
  }
}