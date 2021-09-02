using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeidelbergCement.Service.Interface
{
    public interface IUserRepository<T>
    {
        Task<T> GetUserAsync(string Name);
        Task<IEnumerable<T>> GetUsersAsync();
    }
}
