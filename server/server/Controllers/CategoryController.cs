using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Helpers;
using Server.Models;

namespace Server.Controllers
{
  /// <summary>
  /// Category actions
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class CategoryController : ControllerBase
  {
    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly UnitOfWork _unitOfWork;

    public CategoryController(IHostingEnvironment hostingEnvironment, UnitOfWork unitOfWork)
    {
      _hostingEnvironment = hostingEnvironment;
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Get categories completely
    /// </summary>
    [HttpGet("allcategories")]
    public async Task<List<CategoryResult>> GetAllCategories()
    {
      return await _unitOfWork.Categories.GetAll().Select(c => new CategoryResult(c.Id, c.Name, c.ImageFileName)).ToListAsync();
    }

    /// <summary>
    /// Getting categories according to pagination
    /// </summary>
    /// <param name="searchTerm">for term selection</param>
    /// <param name="pageIndex">for pagination</param>
    /// <param name="pageSize">for pagination</param>
    /// <returns>Category pagination data</returns>
    [HttpGet("categories")]
    public async Task<PaginationResult<CategoryResult>> GetCategories(string searchTerm = "", int pageIndex = 0, int pageSize = 10)
    {
      IQueryable<Category> categories = _unitOfWork.Categories.GetAll().Where(p => p.Name.Contains(searchTerm)).OrderBy(c => c.Id);
      var count = await categories.CountAsync();
      var items = await categories.Skip(pageIndex * pageSize).Take(pageSize).Select(c =>
          new CategoryResult(c.Id, c.Name, c.ImageFileName)).ToListAsync();

      return new PaginationResult<CategoryResult>(items, count);
    }

    /// <summary>
    /// Create category
    /// </summary>
    /// <returns>OK message</returns>
    [Authorize(Roles = "admin")]
    [HttpPost("")]
    public async Task<IActionResult> Add()
    {
      // Addition
      IFormFile file = Request.Form.Files.FirstOrDefault();
      string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
      await AddCategoryAsync(Request, newFileName);
      await FileHelper.AddImageAsync(file, newFileName);      
      return Ok(new { message = StringHelper.AdditionSuccefully });
    }

    /// <summary>
    /// Update category
    /// </summary>
    /// <returns>Success message</returns>
    [Authorize(Roles = "admin")]
    [HttpPut("")]
    public async Task<IActionResult> Update()
    {
      IFormFile file = Request.Form.Files.FirstOrDefault();
      string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
      string oldFileName = await UpdateCategoryAsync(Request, newFileName);
      await FileHelper.UpdateImageAsync(file, oldFileName, newFileName);
      return Ok(new { message = StringHelper.UpdationSuccefully });
    }

    private async Task AddCategoryAsync(HttpRequest request, string newFileName)
    {
      //Category adding DB transaction
      using (var dbContextTransaction = _unitOfWork.BeginTransaction())
      {
        try
        {
          Category category = new Category();          
          category.Name = request.Form["name"];          
          category.ImageFileName = FileHelper.FilterImageName(newFileName);
          await _unitOfWork.Categories.CreateAsync(category);
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

    private async Task<string> UpdateCategoryAsync(HttpRequest request, string newFileName)
    {
      string oldFileName = null;

      // Category updating DB transaction
      using (var dbContextTransaction = _unitOfWork.BeginTransaction())
      {
        try
        {
          Category category = await _unitOfWork.Categories.GetAll().AsNoTracking().SingleOrDefaultAsync(c => c.Id == Int64.Parse(request.Form["id"]));
          oldFileName = category.ImageFileName;          
          category.Name = request.Form["name"];          

          if (!string.IsNullOrEmpty(newFileName))
            category.ImageFileName = newFileName;
          
          _unitOfWork.Categories.Update(category);
          await _unitOfWork.SaveAsync();
          dbContextTransaction.Commit();
        }
        catch (Exception e)
        {
          dbContextTransaction.Rollback(); // Rollbacking DB       
          throw new ApplicationException("DB Transaction Failed. " + e.Message);
        }
      }

      return oldFileName;
    }

  }
}
