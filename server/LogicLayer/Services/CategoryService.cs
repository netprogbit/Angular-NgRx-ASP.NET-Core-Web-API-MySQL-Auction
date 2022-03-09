using LogicLayer.Helpers;
using LogicLayer.Interfaces;
using LogicLayer.InterfacesOut.Auction;
using LogicLayer.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IAuctionUnitOfWork _auctionUnitOfWork;

        public CategoryService(IAuctionUnitOfWork auctionUnitOfWork)
        {
            _auctionUnitOfWork = auctionUnitOfWork;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync()
        {
            var categories = await _auctionUnitOfWork.Categories.FindAllAsync();
            var sortCategories = categories.OrderBy(c => c.Name);
            return sortCategories;
        }

        public async Task<PaginationModel<CategoryModel>> GetCategoryPageAsync(string searchTerm, int pageIndex, int pageSize)
        {
            var paginationModel = await _auctionUnitOfWork.Categories.GetCategoryPageAsync(searchTerm, pageIndex, pageSize);
            return paginationModel;
        }

        public async Task AddCategoryAsync(CategoryModel categoryModel)
        {
            IFormFile file = categoryModel.Image;
            string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
            categoryModel.ImageFileName = FileHelper.FilterImageName(newFileName);
            await _auctionUnitOfWork.Categories.AddAsync(categoryModel);
            await _auctionUnitOfWork.SaveAsync();
            await FileHelper.AddImageAsync(file, newFileName);

        }

        public async Task UpdateCategoryAsync(CategoryModel categoryModel)
        {
            IFormFile file = categoryModel.Image;
            string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
            string oldFileName = null;
            var currentCategoryModel = await _auctionUnitOfWork.Categories.FindByIdAsync(categoryModel.Id);
            oldFileName = currentCategoryModel.ImageFileName;
            currentCategoryModel.Name = categoryModel.Name;

            if (!string.IsNullOrEmpty(newFileName))
                currentCategoryModel.ImageFileName = newFileName;

            _auctionUnitOfWork.Categories.Update(currentCategoryModel);
            await _auctionUnitOfWork.SaveAsync();
            await FileHelper.UpdateImageAsync(file, oldFileName, newFileName);
        }
    }
}
