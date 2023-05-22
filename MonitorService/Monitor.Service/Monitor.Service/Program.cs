using MassTransit;
using Microsoft.Extensions.Hosting;
using Monitor.Service;
using Monitor.Service.Consumers;
using Serilog;
using System.Reflection;
using IHost = Microsoft.Extensions.Hosting.IHost;

//IHost host = Host.CreateDefaultBuilder(args)
//        .UseSerilog((context, logger) =>
//    {
//        logger
//            .MinimumLevel.Debug()
//            .Enrich.FromLogContext();
//        if (context.HostingEnvironment.IsDevelopment())
//        {
//            logger.WriteTo.Console(
//                outputTemplate:
//                "{Timestamp:yyyy-MM-dd HH:mm:ss} {CorrelationId} {Level:u3} {Message}{NewLine}{Exception}");
//        }
//        else
//        {
//            logger.WriteTo.Console();
//        }
//    })
//    .Build();
    
var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint("transaction-monitor-event", e =>
    {
        e.Consumer<TransactionMonitorConsumer>();
    });

});

await busControl.StartAsync(new CancellationToken());
//await host.RunAsync();
Console.ReadLine();

// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
