using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using UserService.Api.JWTWebAuthentication;
using UserService.Models;

namespace UserService.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IJWTManagerRepository _jWTManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IJWTManagerRepository jWTManager, ILogger<UsersController> logger)
        {
            this._jWTManager = jWTManager;
            _logger = logger;
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
    }
}
