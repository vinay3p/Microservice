using MassTransit;
using Newtonsoft.Json;
using SharedLibrary;
using static SharedLibrary.Enumeration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Notifications.Service.Consumers
{
    public class TransactionNotificationConsumer : IConsumer<TransactionGenerated>
    {
        private string connectionString = "Data Source=localhost; Initial Catalog=BankingSystem; User ID=sa; Password=gmed";

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
                                          Amount = context.Message.Amount
                                      },
                                      commandType: CommandType.StoredProcedure);
            }
        }

        private string GenerateNotificationMessageText(ConsumeContext<TransactionGenerated> context)
        {
            return "Dear Customer,\r\nA withdrawal of Rs. " + context.Message.Amount + " has been made from your Bank account (" + "XXXXX" + context.Message.AccountNumber.Substring(5) + ") on " + context.Message.CreatedDate + ". If you did not authorize this transaction, please contact us immediately.\r\nThank you";
        }
    }
}
