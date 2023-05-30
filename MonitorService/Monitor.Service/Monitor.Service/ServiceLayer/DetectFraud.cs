using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SharedLibrary;
using static SharedLibrary.Enumeration;
using Dapper;

namespace Monitor.Service.ServiceLayer
{
    public class DetectFraud
    {
        private string connectionString = "Server=.;Database=MonitorService;Trusted_Connection=True;TrustServerCertificate=True";
        public async Task ReportFraud(TransactionGenerated transaction) {
            using (var connection = new SqlConnection(connectionString))
            {
                var spName = "FraudInsert";
                connection.Open();
                await connection.ExecuteAsync(spName,
                                      new
                                      {
                                          AccountNumber = transaction.AccountNumber,
                                          TransactionTypeID = transaction.TransactionType,
                                          TransferToAccountNumber = transaction.TransferToAccountNumber,
                                          Amount = transaction.Amount,
                                          Message = GenerateMessageText(transaction),
                                          CreatedDate = transaction.CreatedDate = DateTime.Now,
                                      },
                                      commandType: CommandType.StoredProcedure);
            }
        }

        private string GenerateMessageText(TransactionGenerated transaction)
        {
            StringBuilder sb = new StringBuilder();
            switch (transaction.TransactionType)
            {
                case TransactionType.Deposit:
                    return sb.AppendFormat("Dear Customer,\r\nA Deposit of Rs. {0} has been detected from your Bank account (XXXXX{1}) on {2} .If you did not authorize this transaction, please contact us immediately.\r\nThank you", transaction.Amount, transaction.AccountNumber.Substring(5), transaction.CreatedDate).ToString();
                case TransactionType.Withdrawl:
                    return sb.AppendFormat("Dear Customer,\r\nA withdrawal of Rs. {0} has been made from your Bank account (XXXXX{1}) on {2}. If you did not authorize this transaction, please contact us immediately.\r\nThank you", transaction.Amount, transaction.AccountNumber.Substring(5), transaction.CreatedDate).ToString();
                case TransactionType.Transfer:
                    return sb.AppendFormat("Dear Customer,\r\nA Transfer of Rs. {0} has been made from your Bank account (XXXXX{1}) to (XXXXX{2}) on {3}. If you did not authorize this transaction, please contact us immediately.\r\nThank you", transaction.Amount, transaction.AccountNumber.Substring(5), transaction.TransferToAccountNumber.Substring(5), transaction.CreatedDate).ToString();
                default:
                    return string.Empty;
            }
        }

        public async Task CheckFraud(TransactionGenerated transaction) 
        { 
            if ( transaction.Amount >= 20000)
            {
                await ReportFraud(transaction);
            }
        }
    }
}
