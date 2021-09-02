using HeidelbergCement.Data.Models;
using HeidelbergCement.Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeidelbergCement.Service.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        readonly List<User> users;
        public UserRepository()
        {
            users = new List<User>()
            {
                new User{ID=1,Name="admin",Password="123" },
                new User{ID=2,Name="John",Password="2345" }
            };
        }
        public async Task<User> GetUserAsync(string Name)
        {
            return await Task.FromResult(users.FirstOrDefault(x => x.Name == Name));
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await Task.FromResult(users);
        }
    }
}
