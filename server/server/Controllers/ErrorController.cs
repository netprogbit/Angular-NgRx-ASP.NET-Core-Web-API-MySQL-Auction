using LogicLayer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.DTOs;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        protected readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Log error
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("log")]
        public IActionResult Log([FromBody] ErrorDTO errorDTO)
        {
            _logger.LogError("{0} {1}", StringHelper.ClientError, errorDTO.Message);
            return Ok();
        }
    }
}
