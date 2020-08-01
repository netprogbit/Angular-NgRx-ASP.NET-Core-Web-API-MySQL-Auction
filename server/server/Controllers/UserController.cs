using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Helpers;
using Server.Models;

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
    private readonly UnitOfWork _unitOfWork;

    public UserController(UnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Getting users according to pagination
    /// </summary>
    /// <param name="searchTerm">for term selection</param>
    /// <param name="pageIndex">for pagination</param>
    /// <param name="pageSize">for pagination</param>
    /// <returns>User pagination data</returns>
    [HttpGet("users")]
    public async Task<PaginationResult<UserResult>> GetUsers(string searchTerm = "", int pageIndex = 0, int pageSize = 10)
    {
      IQueryable<User> users = _unitOfWork.Users.GetAll().Where(u => u.LastName.Contains(searchTerm)).OrderBy(u => u.Id);
      var count = await users.CountAsync();
      var items = await users.Skip(pageIndex * pageSize).Take(pageSize).Select(u =>
          new UserResult(u.Id, u.FirstName, u.LastName, u.Email, u.Role)).OrderBy(p => p.Id).ToListAsync();

      return new PaginationResult<UserResult>(items, count);
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <returns>Success message</returns>
    [Authorize(Roles = "admin")]
    [HttpPut("")]
    public async Task<IActionResult> Update()
    {
      IFormFile file = Request.Form.Files.FirstOrDefault();
      await UpdateUserAsync(Request);
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
      await DeleteUsertAsync(id);  // Delete user from DB      
      return Ok(new { message = StringHelper.DeleteSuccefully });
    }   

    private async Task UpdateUserAsync(HttpRequest request)
    {
      // User updating DB transaction
      using (var dbContextTransaction = _unitOfWork.BeginTransaction())
      {
        try
        {
          User user = await _unitOfWork.Users.GetAll().AsNoTracking().SingleOrDefaultAsync(u => u.Id == Int64.Parse(request.Form["id"]));
          user.FirstName = request.Form["firstName"];
          user.LastName = request.Form["lastName"];
          user.Email = request.Form["email"];          
          user.Role = request.Form["role"];
          _unitOfWork.Users.Update(user);
          await _unitOfWork.SaveAsync();
          dbContextTransaction.Commit();
        }
        catch (Exception e)
        {
          dbContextTransaction.Rollback(); // Rollbacking DB       
          throw new ApplicationException("DB Transaction Failed. " + e.Message);
        }
      }
    }

    private async Task DeleteUsertAsync(long id)
    {
      // User deleting DB transaction
      using (var dbContextTransaction = _unitOfWork.BeginTransaction())
      {
        try
        {
          _unitOfWork.Users.Delete(id);
          await _unitOfWork.SaveAsync();
          dbContextTransaction.Commit();
        }
        catch (Exception e)
        {
          dbContextTransaction.Rollback(); // Rollbacking DB       
          throw new ApplicationException("DB Transaction Failed. " + e.Message);
        }
      }
    }

  }
}
