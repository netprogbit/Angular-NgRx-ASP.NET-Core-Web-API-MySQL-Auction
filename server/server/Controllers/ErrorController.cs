using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Helpers;
using Server.Models;

namespace Server.Controllers
{
  /// <summary>
  /// Error handler features
  /// </summary>  
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
    public IActionResult Log(ErrorData errorData)
    {
      _logger.LogError("{0} {1}", StringHelper.ClientError, errorData.Message);
      return Ok();
    }

  }
}
