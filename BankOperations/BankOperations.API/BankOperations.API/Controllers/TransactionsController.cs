using BankOperations.Contracts;
using BankOperations.Repository;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/transactions/deposit")]
        public async Task<IActionResult> Deposit(TransactionGenerated transactionGenerated)
        {
            transactionGenerated.CreatedDate = DateTime.Now;
            transactionGenerated.CustomerId = new Guid(User.Claims.ToList().Where(x => x.Type == "id").FirstOrDefault().Value);
            transactionGenerated.TransactionType = Enumeration.TransactionType.Deposit;
            new AccountRepository().Deposit(transactionGenerated);
            _logger.LogInformation($"Deposit type Transaction generated against the account {transactionGenerated.AccountNumber}");

            //await Policy
            //    .Handle<Exception>()
            //    .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
            //                       medianFirstRetryDelay: TimeSpan.FromSeconds(1),
            //                       retryCount: 5))
            //    .ExecuteAsync(async () => publishtoqueue(transactionGenerated));
            var identities = User.Identities;
            await _publishEndpoint.Publish<TransactionGenerated>(transactionGenerated);

            return Ok();
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/transactions/withdrawl")]
        public async Task<IActionResult> Withdrawl(TransactionGenerated transactionGenerated)
        {
            transactionGenerated.CreatedDate = DateTime.Now;
            transactionGenerated.TransactionType = Enumeration.TransactionType.Withdrawl;
            transactionGenerated.CustomerId = new Guid(User.Claims.ToList().Where(x => x.Type == "id").FirstOrDefault().Value);
            new AccountRepository().Withdrawl(transactionGenerated);
            _logger.LogInformation($"A withdrawl type Transaction generated against the account {transactionGenerated.AccountNumber}");

            await _publishEndpoint.Publish<TransactionGenerated>(transactionGenerated);

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/transactions/transfer")]
        public async Task<IActionResult> Transfer(TransactionGenerated transactionGenerated)
        {
            transactionGenerated.CreatedDate = DateTime.Now;
            transactionGenerated.TransactionType = Enumeration.TransactionType.Transfer;
            transactionGenerated.CustomerId = new Guid(User.Claims.ToList().Where(x => x.Type == "id").FirstOrDefault().Value);
            new AccountRepository().Transfer(transactionGenerated);

            await _publishEndpoint.Publish<TransactionGenerated>(transactionGenerated);

            _logger.LogInformation($"Amount Transferred from the account {transactionGenerated.AccountNumber} to the account {transactionGenerated.TransferToAccountNumber}");
            return Ok();
        }
    }
}