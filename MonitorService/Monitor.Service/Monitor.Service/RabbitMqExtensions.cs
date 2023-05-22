using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Service
{
    internal static class RabbitMqExtensions
    {
        public static IRabbitMqBusFactoryConfigurator ConfigureRabbitMq(this IRabbitMqBusFactoryConfigurator rfc, IConfiguration configuration)
        {
            MassTransitRabbitMqConfig rabbitConfig = configuration.GetSection("fwkConfiguration:RabbitMQ").Get<MassTransitRabbitMqConfig>();
            rfc.ConfigureRabbitMq(() => rabbitConfig);
            return rfc;
        }

        public static IRabbitMqBusFactoryConfigurator ConfigureRabbitMq(this IRabbitMqBusFactoryConfigurator rfc, Func<IMassTransitRabbitMqConfig> mtConfigFactory)
        {
            IMassTransitRabbitMqConfig config = mtConfigFactory();
            UriBuilder uriBuilder = new UriBuilder(config.Scheme, config.Host, config.Port, config.VirtualHost);
            //rfc.Host(new RabbitMqHostSettings {})
            //rfc.Host(uriBuilder.Uri, delegate (IRabbitMqHostConfigurator h)
            //{
            //    h.Username(config.Username);
            //    h.Password(config.Password);
            //});
            return rfc;
        }
    }

    public class MassTransitRabbitMqConfig : IMassTransitRabbitMqConfig
    {
        public string Host { get; set; } = "localhost";


        public ushort Port { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string VirtualHost { get; set; } = "/";


        public string Scheme { get; set; }
    }

    public interface IMassTransitRabbitMqConfig
    {
        string Host { get; set; }

        ushort Port { get; set; }

        string Password { get; set; }

        string Username { get; set; }

        string VirtualHost { get; set; }

        string Scheme { get; set; }
    }
}
