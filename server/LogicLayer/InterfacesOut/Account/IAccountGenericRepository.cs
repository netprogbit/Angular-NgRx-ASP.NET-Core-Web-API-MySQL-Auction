using LogicLayer.Models.Interfaces;
using System.Threading.Tasks;

namespace LogicLayer.InterfacesOut.Account
{
    public interface IAccountGenericRepository<TModel> : IGenericRepository<TModel>
        where TModel: class, IAccountModel
    {
        Task<TModel> FindByIdAsync(string id);
        void Update(TModel model);
        Task DeleteByIdAsync(string id);
    }
}
