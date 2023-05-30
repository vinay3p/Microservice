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
using System.Diagnostics.Eventing.Reader;
using Serilog;

namespace Notifications.Service.Consumers
{
    public class TransactionNotificationConsumer : IConsumer<TransactionGenerated>
    {
        private string connectionString = "Data Source=localhost; Initial Catalog=NotificationService; User ID=sa; Password=gmed";
        private readonly ILogger<TransactionNotificationConsumer> _logger;
        Serilog.Core.Logger _seriLogger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("d:\\log.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: null, retainedFileCountLimit: null, shared: true, flushToDiskInterval: TimeSpan.FromSeconds(1)).CreateLogger();

        public async Task Consume(ConsumeContext<TransactionGenerated> context)
        {
            _seriLogger.Information("\r\n" + DateTime.Now + " - Consume Message Execution started");
            _seriLogger.Information("\r\n" + DateTime.Now + " - Parameters - " + JsonConvert.SerializeObject(context.Message));

            using (var connection = new SqlConnection(connectionString))
            {
                var spName = "NotificationsInsert";
                connection.Open();
                await connection.ExecuteAsync(spName,
                                       new
                                       {
                                           CustomerId = context.Message.CustomerId,
                                           Message = GenerateNotificationMessageText(context),
                                           NotificationDate = context.Message.CreatedDate = DateTime.Now,
                                           AccountNumber = context.Message.AccountNumber,
                                           TransactionTypeID = context.Message.TransactionType,
                                           Amount = context.Message.Amount,
                                           TransferToAccountNumber = context.Message.TransferToAccountNumber
                                       },
                                       commandType: CommandType.StoredProcedure);
            }
            _seriLogger.Information("\r\n" + DateTime.Now + " - Consume Message Execution End");
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
