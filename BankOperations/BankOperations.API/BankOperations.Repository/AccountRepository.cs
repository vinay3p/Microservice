using BankOperations.Contracts;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using static BankOperations.Contracts.Enumeration;

namespace BankOperations.Repository
{
    public class AccountRepository : IRepository<TransactionGenerated>
    {
        private IDbConnection dbConnection = null;
        //private readonly IConfiguration = null;
        private string connectionString = "Data Source=localhost; Initial Catalog=BankingSystem; User ID=sa; Password=gmed";

        public AccountRepository()
        {
        }

        public void Deposit(TransactionGenerated transactionGenerated)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var spName = "TransactionsDeposit";
                connection.Open();
                connection.Execute(spName,
                                      new
                                      {
                                          AccountNumber = transactionGenerated.AccountNumber,
                                          TransactionDate = transactionGenerated.CreatedDate,
                                          Amount = transactionGenerated.Amount,
                                          TransactionTypeID = TransactionType.Deposit,
                                          CustomerId = transactionGenerated.CustomerId
                                      },
                                      commandType: CommandType.StoredProcedure);
            }
        }

        public void Withdrawl(TransactionGenerated transactionGenerated)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var spName = "TransactionsWithdrawl";
                connection.Open();
                connection.Execute(spName,
                                      new
                                      {
                                          AccountNumber = transactionGenerated.AccountNumber,
                                          TransactionDate = transactionGenerated.CreatedDate,
                                          Amount = transactionGenerated.Amount,
                                          TransactionTypeID = TransactionType.Withdrawl,
                                          CustomerId = transactionGenerated.CustomerId
                                      },
                                      commandType: CommandType.StoredProcedure);
            }
        }

        public void Transfer(TransactionGenerated transactionGenerated)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var spName = "TransactionsTransfer";
                connection.Open();
                connection.Execute(spName,
                                      new
                                      {
                                          AccountNumber = transactionGenerated.AccountNumber,
                                          TransactionDate = transactionGenerated.CreatedDate,
                                          Amount = transactionGenerated.Amount,
                                          TransactionTypeID = TransactionType.Transfer,
                                          CustomerId = transactionGenerated.CustomerId,
                                          TransferToAccountNumber = transactionGenerated.TransferToAccountNumber
                                      },
                                      commandType: CommandType.StoredProcedure);
            }
        }
    }
}