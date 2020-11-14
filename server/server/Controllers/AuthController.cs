using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
  /// <summary>
  /// Authentication actions
  /// </summary>
  [AllowAnonymous]
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }


    /// <summary>
    /// Registration in the system
    /// </summary>
    /// <returns>Success message</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {      
      bool isRegistered = await _authService.RegisterAsync(user);

      if (!isRegistered)
        return BadRequest(new { message = StringHelper.EmailExists });

      return Ok(new { message = StringHelper.RegistrationSuccefully });
    }

    /// <summary>
    /// Logging in
    /// </summary>
    /// <returns>Token data</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(User user)
    {
      TokenResult tokenResult = await _authService.LoginAsync(user.Email, user.Password);

      if (tokenResult == null)
        return NotFound(new { message = StringHelper.UserNotFound });

      return Ok(tokenResult);
    }
  }
}
