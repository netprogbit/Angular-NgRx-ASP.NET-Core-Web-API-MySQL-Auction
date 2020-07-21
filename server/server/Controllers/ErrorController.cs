using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Helpers;

namespace Server.Controllers
{
  /// <summary>
  /// Error handler features
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class ErrorController : ControllerBase
  {
    protected readonly ILogger<ProductController> _logger;

    public ErrorController(ILogger<ProductController> logger)
    {
      _logger = logger;
    }

    /// <summary>
    /// Log error
    /// </summary>
    /// <returns></returns>
    [HttpPost("log")]
    public IActionResult Log()
    {
      _logger.LogError("{0} {1}", StringHelper.ClientError, Request.Form["message"]);
      return Ok();
    }

  }
}
