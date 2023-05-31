using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using SharedLibrary;
using UserService.Api.JWTWebAuthentication;
using UserService.Models;
using UserService.Repository;
using MassTransit;

namespace UserService.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IJWTManagerRepository _jWTManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public UsersController(IJWTManagerRepository jWTManager, ILogger<UsersController> logger, IPublishEndpoint publishEndpoint)
        {
            this._jWTManager = jWTManager;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public List<string> Get()
        {
            var users = new List<string>
        {
            "Satinder Singh",
            "Amit Sarna",
            "Davin Jon"
        };

            return users;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(UserLogin usersdata)
        {
            var token = _jWTManager.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }
            _logger.LogInformation($"Token generated for user {usersdata.UserName}");
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(User user)
        {
            new UserRepository().CreateUser(user);
            _logger.LogInformation($"User {0} with ID {1} synced between User and Banking Service", user.Name, user.UserId);
            await _publishEndpoint.Publish<User>(user);

            return Ok();
        }

    }
}
