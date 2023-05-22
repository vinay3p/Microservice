using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;
using Newtonsoft.Json;

namespace BankOperations.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ILogger<TransactionsController> logger, IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public void Get()
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            //_publishEndpoint.Publish<TransactionGenerated>(Newtonsoft.Json.JsonConvert.SerializeObject(new TransactionGenerated()));

            _publishEndpoint.Publish<TransactionGenerated>(new TransactionGenerated
            {
                AccountId = accountId,
                Amount = 50000,
                CreatedDate = DateTime.Now,
                TransactionType = Enumeration.TransactionType.Deposit,
                UserId = userId
            });
        }

        [HttpPost]
        [Route("api/transactions/deposit")]
        public async Task<IActionResult> Deposit(TransactionGenerated transactionGenerated)
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            await _publishEndpoint.Publish<TransactionGenerated>(Newtonsoft.Json.JsonConvert.SerializeObject(new TransactionGenerated()));

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