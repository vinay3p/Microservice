using MassTransit;
using Notifications.Service.Consumers;

var builder = WebApplication.CreateBuilder(args);

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint("transaction-generated-event", e =>
    {
        e.Consumer<TransactionNotificationConsumer>();
    });

});

await busControl.StartAsync(new CancellationToken());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
