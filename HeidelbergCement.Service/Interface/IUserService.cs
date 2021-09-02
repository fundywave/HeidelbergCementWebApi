using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeidelbergCement.Service.Interface
{
    public interface IUserService<T>
    {
        Task<IEnumerable<T>> GetUsers();
    }
}
