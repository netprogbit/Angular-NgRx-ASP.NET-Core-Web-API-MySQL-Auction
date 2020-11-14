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
    /// User actions
    /// </summary>
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Getting users according to pagination
        /// </summary>
        /// <param name="searchTerm">for term selection</param>
        /// <param name="pageIndex">for pagination</param>
        /// <param name="pageSize">for pagination</param>
        /// <returns>User pagination data</returns>
        [HttpGet("users")]
        public async Task<PaginationResult<UserResult>> GetUsers(string searchTerm = "", int pageIndex = 0, int pageSize = 10) => await _userService.GetUsersAsync(searchTerm, pageIndex, pageSize);

        /// <summary>
        /// Update user
        /// </summary>
        /// <returns>Success message</returns>
        [Authorize(Roles = "admin")]
        [HttpPut("")]
        public async Task<IActionResult> Update(User user)
        {
            await _userService.UpdateUserAsync(user);
            return Ok(new { message = StringHelper.UpdationSuccefully });
        }

        /// <summary>
        /// Delete user by Id
        /// </summary>
        /// <returns>Succcess message</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = StringHelper.DeleteSuccefully });
        }
    }
}
