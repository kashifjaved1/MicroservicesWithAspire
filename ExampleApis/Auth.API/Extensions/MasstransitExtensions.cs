using MassTransit;
using Shared.Events;

namespace Auth.API.Extensions
{
    public static class MasstransitExtensions
    {
        public static IServiceCollection ConfigureMasstransit(this IServiceCollection services, string exchangeName, string exchangeType, string routingKey, string queueName)
        {
            // In RabbitMQ we register the ConnectionFactory(), and in Masstransit we configure requestClient(s), endpoint(s) and consumer(s).
            services.AddMassTransit(x =>
            {
                //x.SetKebabCaseEndpointNameFormatter();
                //x.SetSnakeCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, rabbitMqBusFactoryConfigurator) =>
                {
                    rabbitMqBusFactoryConfigurator.Host("localhost", "/", h =>
                    {
                        var defaultCredential = "guest";
                        h.Username(defaultCredential);
                        h.Password(defaultCredential);
                    });

                    //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    //rabbitMqBusFactoryConfigurator.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter(environment));

                    // NOTE: You can play with stuff like EndpointNameFormatter & EntityNameFormatter and things I did below but its better to use default and let the masstransit handle the complexity stuff rathere than headbutting on your own.
                    // Tried following but it didn't worked. Even everything there it gives following error ('PRECONDITION_FAILED - inequivalent arg 'type' for exchange 'myCustomExchange' in vhost '/': received 'fanout' but current is 'direct') because I've created direct exchange and the exchange masstransit automaticlaly creates n manage is of fanout type. So fall back to masstransit default setup rathing than increasing complexity.
                    //// Option 1: Completely disable automatic endpoint configuration
                    //rabbitMqBusFactoryConfigurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(prefix: "DEV_", includeNamespace: false));

                    //rabbitMqBusFactoryConfigurator.ReceiveEndpoint(queueName, cfg =>
                    //{
                    //    cfg.ConfigureConsumeTopology = false;
                    //    cfg.Bind(exchangeName, bind =>
                    //    {
                    //        bind.ExchangeType = exchangeType;
                    //        bind.RoutingKey = routingKey;
                    //        bind.Durable = true;
                    //        bind.AutoDelete = false;
                    //    });
                    //});

                    //// Prevent automatic exchange creation for GrpcCallProcessed
                    //rabbitMqBusFactoryConfigurator.Send<GrpcCallProcessed>(x =>
                    //{
                    //    x.UseRoutingKeyFormatter(_ => routingKey);
                    //});

                    //// Optional: Explicitly configure the message to use your custom exchange
                    //rabbitMqBusFactoryConfigurator.Message<GrpcCallProcessed>(x =>
                    //{
                    //    x.SetEntityName(exchangeName);
                    //});
                });

                x.AddRequestClient(typeof(GrpcCallProcessed));
            });

            return services;
        }
    }   
}
