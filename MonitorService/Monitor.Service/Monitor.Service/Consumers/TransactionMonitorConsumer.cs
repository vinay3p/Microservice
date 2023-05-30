﻿using MassTransit;
using Monitor.Service.ServiceLayer;
using Newtonsoft.Json;
using SharedLibrary;

namespace Monitor.Service.Consumers
{
    public class TransactionMonitorConsumer : IConsumer<TransactionGenerated>
    {
        private string connectionString = "Server=.;Database=MonitorService;Trusted_Connection=True;TrustServerCertificate=True";

        public async Task Consume(ConsumeContext<TransactionGenerated> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"Transaction Generated message: {jsonMessage}");

            await new DetectFraud().CheckFraud(context.Message);
        }
    }
}
