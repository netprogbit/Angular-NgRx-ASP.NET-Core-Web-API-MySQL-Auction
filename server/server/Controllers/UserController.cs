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
    /// User actions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Getting products according to pagination
        /// </summary>
        /// <param name="searchTerm">for term selection</param>
        /// <param name="pageIndex">for pagination</param>
        /// <param name="pageSize">for pagination</param>
        /// <returns>Product pagination data</returns>
        [HttpGet("users")]
        public async Task<PaginationDTO<UserDTO>> GetUserPage(string searchTerm = "", int pageIndex = 0, int pageSize = 10)
        {
            var paginationModel = await _userService.GetUserPageAsync(searchTerm, pageIndex, pageSize);
            var paginationDTO = _mapper.Map<PaginationDTO<UserDTO>>(paginationModel);
            return paginationDTO;
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <returns>Success message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public async Task<IActionResult> Update(UserDTO userDTO)
        {
            var userModel = _mapper.Map<UserModel>(userDTO);
            await _userService.UpdateUserAsync(userModel);
            return Ok(StringHelper.UpdationSuccessfully);
        }

        /// <summary>
        /// Delete user by Id
        /// </summary>
        /// <returns>Succcess message</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.DeleteUserByIdAsync(id);
            return Ok(StringHelper.DeleteSuccefully);
        }
    }
}
