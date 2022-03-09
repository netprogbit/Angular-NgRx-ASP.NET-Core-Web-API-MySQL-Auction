using LogicLayer.Interfaces;
using LogicLayer.InterfacesOut.Account;
using LogicLayer.Models;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountUnitOfWork _userUnitOrWork;

        public UserService(IAccountUnitOfWork userUnitOrWork)
        {
            _userUnitOrWork = userUnitOrWork;
        }

        public async Task<PaginationModel<UserModel>> GetUserPageAsync(string searchTerm, int pageIndex, int pageSize)
        {
            var paginationModel = await _userUnitOrWork.Users.GetUserPageAsync(searchTerm, pageIndex, pageSize);
            return paginationModel;
        }

        public async Task UpdateUserAsync(UserModel userModel)
        {
            UserModel currUserModel = await _userUnitOrWork.Users.FindByIdAsync(userModel.Id);
            currUserModel.UserName = userModel.UserName;
            _userUnitOrWork.Users.Update(currUserModel);
            await _userUnitOrWork.SaveAsync();
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            await _userUnitOrWork.Users.DeleteByIdAsync(id);
            await _userUnitOrWork.SaveAsync();
        }
    }
}
