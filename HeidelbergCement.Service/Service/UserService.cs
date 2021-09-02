using HeidelbergCement.Service.Interface;
using HeidelbergCement.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeidelbergCement.Service.Service
{
    public class UserService : IUserService<User>
    {
        readonly IUserRepository<User> _userRepository;
        public UserService(IUserRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await (await Task.FromResult(_userRepository.GetUsersAsync())).ConfigureAwait(false);
        }
    }
}
