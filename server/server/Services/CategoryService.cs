using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Server.Helpers;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class CategoryService
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryResult>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAll().Select(c => new CategoryResult(c.Id, c.Name, c.ImageFileName)).ToListAsync();
        }

        public async Task<PaginationResult<CategoryResult>> GetCategoriesAsync(string searchTerm, int pageIndex, int pageSize)
        {
            IQueryable<Category> categories = _unitOfWork.Categories.GetAll().Where(p => p.Name.Contains(searchTerm)).OrderBy(c => c.Id);
            var count = await categories.CountAsync();
            var items = await categories.Skip(pageIndex * pageSize).Take(pageSize).Select(c =>
                new CategoryResult(c.Id, c.Name, c.ImageFileName)).ToListAsync();

            return new PaginationResult<CategoryResult>(items, count);
        }

        public async Task AddCategoryAsync(HttpRequest request)
        {
            // Addition
            IFormFile file = request.Form.Files.FirstOrDefault();
            string newFileName = FileHelper.GetUniqueFileName(file?.FileName);

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

            await FileHelper.AddImageAsync(file, newFileName);            
        }

        public async Task UpdateCategoryAsync(HttpRequest request)
        {
            IFormFile file = request.Form.Files.FirstOrDefault();
            string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
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

            await FileHelper.UpdateImageAsync(file, oldFileName, newFileName);
        }
    }
}
