using MassTransit;
using Newtonsoft.Json;
using SharedLibrary;

namespace Notifications.Service.Consumers
{
    public class TransactionNotificationConsumer : IConsumer<TransactionGenerated>
    {
        public async Task Consume(ConsumeContext<TransactionGenerated> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"Transaction Generated message: {jsonMessage}");
        }
    }
}
