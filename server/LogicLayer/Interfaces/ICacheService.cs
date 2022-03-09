using System.Threading.Tasks;

namespace LogicLayer.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key) where T : class;
        Task<T> SetAsync<T>(string key, T value) where T : class;
        Task ClearCacheAsync(string key);
    }
}
