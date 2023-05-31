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
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

namespace Notifications.Service.Consumers
{
    public class UserSyncConsumer : IConsumer<User>
    {
        private string connectionString = "Data Source=localhost; Initial Catalog=BankingSystem; User ID=sa; Password=gmed";
        private readonly ILogger<UserSyncConsumer> _logger;
        Serilog.Core.Logger _seriLogger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("d:\\log.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: null, retainedFileCountLimit: null, shared: true, flushToDiskInterval: TimeSpan.FromSeconds(1)).CreateLogger();

        public async Task Consume(ConsumeContext<User> context)
        {
            try
            {
                _seriLogger.Information("\r\n" + DateTime.Now + " - Consume Message Execution started");
                _seriLogger.Information("\r\n" + DateTime.Now + " - Parameters - " + JsonConvert.SerializeObject(context.Message));

                using (var connection = new SqlConnection(connectionString))
                {
                    var spName = "UserInsert";
                    connection.Open();
                    await connection.ExecuteAsync(spName,
                                           new
                                           {
                                               Id = context.Message.Id,
                                               UserId = context.Message.UserId,
                                               Name = context.Message.Name
                                           },
                                           commandType: CommandType.StoredProcedure);
                }
                _seriLogger.Information("\r\n" + DateTime.Now + " - Consume Message Execution End");
            }
            catch (Exception ex) { }
        }
    }
}