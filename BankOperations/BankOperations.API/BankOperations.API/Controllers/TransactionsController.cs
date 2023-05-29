using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BankOperations.API.Controllers
{
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ILogger<TransactionsController> logger, IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }


        [HttpPost("api/transactions/getname1")]
        public String GetName1()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                }
                return "Valid";
            }
            else
            {
                return "Invalid";
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/transactions/deposit")]
        public async Task<IActionResult> Deposit(TransactionGenerated transactionGenerated)
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            await _publishEndpoint.Publish<TransactionGenerated>(Newtonsoft.Json.JsonConvert.SerializeObject(transactionGenerated));

            //_publishEndpoint.Publish<TransactionMoitor>(new TransactionGenerated
            //{
            //    AccountId = accountId,
            //    Amount = 50000,
            //    CreatedDate = DateTime.Now,
            //    TransactionType = Enumeration.TransactionType.Deposit,
            //    UserId = userId
            //});
            return Ok();
        }
    }
}