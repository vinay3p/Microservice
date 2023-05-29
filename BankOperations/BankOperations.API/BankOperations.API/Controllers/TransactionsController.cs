using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BankOperations.Repository;

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
            transactionGenerated.TransactionType = Enumeration.TransactionType.Deposit;
            new AccountRepository().Deposit(transactionGenerated);

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
            new AccountRepository().Withdrawl(transactionGenerated);

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
            new AccountRepository().Transfer(transactionGenerated);

            await _publishEndpoint.Publish<TransactionGenerated>(transactionGenerated);

            return Ok();
        }
    }
}