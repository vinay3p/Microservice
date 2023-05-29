using MassTransit;
using Newtonsoft.Json;
using SharedLibrary;
using static SharedLibrary.Enumeration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq.Expressions;
using System.Text;
using System.Diagnostics;

namespace Notifications.Service.Consumers
{
    public class TransactionNotificationConsumer : IConsumer<TransactionGenerated>
    {
        private string connectionString = "Data Source=localhost; Initial Catalog=NotificationService; User ID=sa; Password=gmed";

        public async Task Consume(ConsumeContext<TransactionGenerated> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"Transaction Generated message: {jsonMessage}");

            using (var connection = new SqlConnection(connectionString))
            {
                var spName = "NotificationsInsert";
                connection.Open();
                connection.Execute(spName,
                                      new
                                      {
                                          CustomerId = context.Message.CustomerId,
                                          Message = GenerateNotificationMessageText(context),
                                          NotificationDate = context.Message.CreatedDate = DateTime.Now,
                                          AccountNumber = context.Message.AccountNumber,
                                          TransactionTypeID = TransactionType.Deposit,
                                          Amount = context.Message.Amount,
                                          TransferToAccountNumber = context.Message.TransferToAccountNumber
                                      },
                                      commandType: CommandType.StoredProcedure);
            }
        }

        private string GenerateNotificationMessageText(ConsumeContext<TransactionGenerated> context)
        {
            StringBuilder sb = new StringBuilder();
            switch (context.Message.TransactionType)
            {
                case TransactionType.Deposit:
                    return sb.AppendFormat("Dear Customer,\r\nA Deposit of Rs. {0} has been made from your Bank account (XXXXX{1}) on {2} \r\nThank you", context.Message.Amount, context.Message.AccountNumber.Substring(5), context.Message.CreatedDate).ToString();
                case TransactionType.Withdrawl:
                    return sb.AppendFormat("Dear Customer,\r\nA withdrawal of Rs. {0} has been made from your Bank account (XXXXX{1}) on {2}. If you did not authorize this transaction, please contact us immediately.\r\nThank you", context.Message.Amount, context.Message.AccountNumber.Substring(5), context.Message.CreatedDate).ToString();
                case TransactionType.Transfer:
                    return sb.AppendFormat("Dear Customer,\r\nA Transfer of Rs. {0} has been made from your Bank account (XXXXX{1}) to (XXXXX{2}) on {3}. If you did not authorize this transaction, please contact us immediately.\r\nThank you", context.Message.Amount, context.Message.AccountNumber.Substring(5), context.Message.TransferToAccountNumber.Substring(5), context.Message.CreatedDate).ToString();
                default:
                    return string.Empty;
            }
        }
    }
}
