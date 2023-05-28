using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        public UserRepository(DapperContext context) { _context = context; }

        public async Task<IEnumerable<Users>> GetUsers()
        {
            var query = "SELECT * FROM users";
            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<Users>(query);
                return users.ToList();
            }
        }

        public async Task<Users> GetUser(Users user)
        {
            var query = "SELECT * FROM [user] where userId = @userId and password = @password";
            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QuerySingleOrDefaultAsync<Users>(query, new {userid =user.UserId, password=user.Password });
                return users;
            }
        }

    }
}
