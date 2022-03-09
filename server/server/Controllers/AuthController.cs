using AutoMapper;
using LogicLayer.Helpers;
using LogicLayer.Interfaces;
using LogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using System.Threading.Tasks;

namespace Server.Controllers
{
    /// <summary>
    /// Authentication actions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        /// <summary>
        /// Registration in the system
        /// </summary>
        /// <returns>Success message</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var userModel = _mapper.Map<UserModel>(userDTO);
            bool isRegistered = await _authService.RegisterAsync(userModel);

            if (!isRegistered)
                return BadRequest(StringHelper.EmailExists);

            return Ok(StringHelper.RegistrationSuccessfully);
        }

        /// <summary>
        /// Logging in
        /// </summary>
        /// <returns>Token data</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var userModel = _mapper.Map<UserModel>(userDTO);
            var tokenModel = await _authService.AuthenticateAsync(userModel);

            if (tokenModel == null)
            {
                return BadRequest(StringHelper.UserNotFound);
            }

            var tokenDTO = _mapper.Map<TokenDTO>(tokenModel);            
            return Ok(tokenDTO);
        }

        /// <summary>
        /// Refreshing token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO tokenDTO)
        {
            var tokenModel = _mapper.Map<TokenModel>(tokenDTO);
            var newTokenModel = await _authService.RefreshTokenAsync(tokenModel);
            var newTokenDTO = _mapper.Map<TokenDTO>(newTokenModel);
            return Ok(newTokenDTO);
        }

        /// <summary>
        /// Revoking token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeToken([FromBody] TokenDTO tokenDTO)
        {
            var tokenModel = _mapper.Map<TokenModel>(tokenDTO);
            await _authService.RevokeToken(tokenModel);
            return Ok(StringHelper.LogoutCompleted);
        }
    }
}
