using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;
using SharedLibrary;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        private string connectionString = "Data Source=localhost; Initial Catalog=UserService; User ID=sa; Password=gmed;trustServerCertificate=True;";

        public UserRepository() { }

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

        public async Task<Users> Authenticate(UserLogin user)
        {
            var query = "SELECT * FROM [user] where userId = @userId and password = @password";
            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QuerySingleOrDefaultAsync<Users>(query, new {userid =user.UserName, password=user.Password });
                return users;
            }
        }

        public void CreateUser(User user)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var spName = "UserInsert";
                    connection.Open();
                    connection.Execute(spName,
                                          new
                                          {
                                              Id = user.Id,
                                              UserId = user.UserId,
                                              Name = user.Name,
                                              Password = user.Password,
                                              Email = user.Email,
                                              Mobile = user.Mobile
                                          },
                                          commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
