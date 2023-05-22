using MassTransit;
using Newtonsoft.Json;
using SharedLibrary;

namespace Monitor.Service.Consumers
{
    public class TransactionMonitorConsumer : IConsumer<TransactionGenerated>
    {
        public async Task Consume(ConsumeContext<TransactionGenerated> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"Transaction Generated message: {jsonMessage}");
        }
    }
}
