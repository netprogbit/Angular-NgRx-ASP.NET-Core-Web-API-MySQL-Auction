using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResult<UserResult>> GetUsersAsync(string searchTerm, int pageIndex, int pageSize )
        {
            IQueryable<User> users = _unitOfWork.Users.GetAll().Where(u => u.LastName.Contains(searchTerm)).OrderBy(u => u.Id);
            var count = await users.CountAsync();
            var items = await users.Skip(pageIndex * pageSize).Take(pageSize).Select(u =>
                new UserResult(u.Id, u.FirstName, u.LastName, u.Email, u.Role)).OrderBy(p => p.Id).ToListAsync();

            return new PaginationResult<UserResult>(items, count);
        }

        public async Task UpdateUserAsync(User user)
        {
            // User updating DB transaction
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    User curUser = await _unitOfWork.Users.GetAll().AsNoTracking().SingleOrDefaultAsync(u => u.Id == user.Id);
                    curUser.FirstName = user.FirstName;
                    curUser.LastName = user.LastName;
                    curUser.Email = user.Email;
                    curUser.Role = user.Role;
                    _unitOfWork.Users.Update(curUser);
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

        public async Task DeleteUserAsync(long id)
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
