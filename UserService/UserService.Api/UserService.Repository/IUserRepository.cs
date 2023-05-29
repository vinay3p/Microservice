using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        public Task<IEnumerable<Users>> GetUsers();

        public Task<Users> Authenticate(UserLogin user);
    }
}
