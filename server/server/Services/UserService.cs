using DataLayer.Entities;
using DataLayer.UnitOfWork;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResult<UserResult>> GetUsersAsync(string searchTerm, int pageIndex, int pageSize)
        {
            IEnumerable<User> users = await _unitOfWork.Users.FindAllAsync(u => u.LastName.Contains(searchTerm));
            var count = users.Count();
            var items = users.Skip(pageIndex * pageSize).Take(pageSize).Select(u =>
                new UserResult(u.Id, u.FirstName, u.LastName, u.Email, u.Role)).OrderBy(p => p.Id).ToList();

            return new PaginationResult<UserResult>(items, count);
        }

        public async Task UpdateUserAsync(User user)
        {
            // User updating DB transaction
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    User currUser = await _unitOfWork.Users.FindAsync(user.Id);
                    currUser.FirstName = user.FirstName;
                    currUser.LastName = user.LastName;
                    currUser.Email = user.Email;
                    currUser.Role = user.Role;
                    _unitOfWork.Users.Update(currUser);
                    await _unitOfWork.SaveAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback(); // Rollbacking DB       
                    ExceptionDispatchInfo.Capture(ex).Throw();
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback(); // Rollbacking DB       
                    ExceptionDispatchInfo.Capture(ex).Throw();
                }
            }
        }
    }
}
