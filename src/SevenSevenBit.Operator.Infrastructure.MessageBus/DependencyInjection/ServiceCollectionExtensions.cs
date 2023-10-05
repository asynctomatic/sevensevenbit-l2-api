namespace SevenSevenBit.Operator.Infrastructure.MessageBus.DependencyInjection;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers.Definitions;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Options;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Sagas;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Services;
using SevenSevenBit.Operator.Infrastructure.MessageBus.States;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBusWebInfrastructureServices<TDbContext>(
        this IServiceCollection serviceCollection,
        bool isProduction)
        where TDbContext : DbContext
    {
        // Add services
        serviceCollection.AddScoped<IMessageBusService, MessageBusService>();

        // Add options
        serviceCollection
            .AddOptions<MessageBusOptions>()
            .BindConfiguration(MessageBusOptions.MessageBus)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Load message bus settings from the config file.
        var sp = serviceCollection.BuildServiceProvider();
        var messageBusSettings = sp.GetRequiredService<IOptions<MessageBusOptions>>().Value;

        // Add and configure the MassTransit framework.
        // https://masstransit-project.com/usage/configuration.html#configuration
        serviceCollection.AddMassTransit(busConfigurator =>
        {
            // Configures MassTransit to generate endpoint names using kebab case (e.g. submit-transaction).
            // https://masstransit-project.com/usage/containers/#bus-configuration
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            // TODO
            // Configures Transactional Outbox for MassTransit.
            // https://masstransit-project.com/advanced/transactional-outbox.html
            // busConfigurator.AddEntityFrameworkOutbox<TDbContext>(configurator =>
            // {
            //     // Using default outbox configs for now, we can readjust later if needed, when we have more data to work with.
            //     configurator.UsePostgres();
            //     configurator.UseBusOutbox();
            // });

            // Add TransactionStreamResultConsumer to handle reply from saga
            busConfigurator.AddConsumer<TransactionStreamResultConsumer>(consumerConfigurator =>
            {
                // Configure retry pattern at the consumer level
                consumerConfigurator.UseMessageRetry(retryConfigurator => { retryConfigurator.Immediate(messageBusSettings.Consumers.TransactionStreamResultConsumer.Retry.Limit); });
            });

            void BusConfiguratorAction<T>(IBusRegistrationContext context, IBusFactoryConfigurator<T> busFactoryConfigurator)
                where T : IReceiveEndpointConfigurator
            {
                // Retry options at the bus level
                busFactoryConfigurator.UseMessageRetry(configurator => configurator.Immediate(messageBusSettings.Retry.Limit));

                // Send bus metrics to Prometheus
                busFactoryConfigurator.UsePrometheusMetrics(serviceName: "sevensevenbit-operator-web");

                // Generate queue names for each receive endpoint.
                // https://masstransit-project.com/usage/guidance.html#receive-endpoints
                busFactoryConfigurator.ConfigureEndpoints(context);
            }

            // If environment is not production, use RabbitMQ as the message bus.
            if (!isProduction)
            {
                // Set RabbitMQ as the message bus.
                // https://masstransit-project.com/usage/transports/rabbitmq.html#rabbitmq
                busConfigurator.UsingRabbitMq((ctx, config) =>
                {
                    // Configure the message bus.
                    // https://masstransit-project.com/usage/transports/rabbitmq.html#configuration
                    config.Host(messageBusSettings.RabbitMq.Host, "/", h =>
                    {
                        h.Username(messageBusSettings.RabbitMq.Username);
                        h.Password(messageBusSettings.RabbitMq.Password);
                    });

                    // Common bus configs
                    BusConfiguratorAction(ctx, config);
                });
                return;
            }

            // Set Azure Service Bus as the message bus.
            // https://masstransit.io/documentation/configuration/transports/azure-service-bus
            busConfigurator.UsingAzureServiceBus((ctx, config) =>
            {
                config.Host(messageBusSettings.AzureServiceBus.ConnectionString);

                // Common bus configs
                BusConfiguratorAction(ctx, config);
            });
        });

        return serviceCollection;
    }

    public static IServiceCollection AddMessageBusTransactionIdInfrastructureServices(
        this IServiceCollection serviceCollection,
        bool isProduction)
    {
        // Add services
        serviceCollection.AddScoped<IMessageBusService, MessageBusService>();

        // Add options
        serviceCollection
            .AddOptions<MessageBusOptions>()
            .BindConfiguration(MessageBusOptions.MessageBus)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Load message bus settings from the config file.
        var sp = serviceCollection.BuildServiceProvider();
        var messageBusSettings = sp.GetRequiredService<IOptions<MessageBusOptions>>().Value;

        // Add and configure the MassTransit framework.
        // https://masstransit-project.com/usage/configuration.html#configuration
        serviceCollection.AddMassTransit(busConfigurator =>
        {
            // Configures MassTransit to generate endpoint names using kebab case (e.g. submit-transaction).
            // https://masstransit-project.com/usage/containers/#bus-configuration
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            // Register consumers to handle message bus events.
            // https://masstransit-project.com/usage/configuration.html#consumers
            busConfigurator.AddConsumer<AllocateTransactionIdConsumer, AllocateTransactionIdConsumerDefinition>();
            busConfigurator.AddConsumer<FreeTransactionIdConsumer, FreeTransactionIdConsumerDefinition>();

            void BusConfiguratorAction<T>(IBusRegistrationContext context, IReceiveConfigurator<T> receiveConfigurator)
                where T : IReceiveEndpointConfigurator
            {
                // Generate queue names for each receive endpoint.
                // https://masstransit-project.com/usage/guidance.html#receive-endpoints
                receiveConfigurator.ConfigureEndpoints(context);
            }

            // If environment is not production, use RabbitMQ as the message bus.
            if (!isProduction)
            {
                // Set RabbitMQ as the message bus.
                // https://masstransit-project.com/usage/transports/rabbitmq.html#rabbitmq
                busConfigurator.UsingRabbitMq((ctx, config) =>
                {
                    // Configure the message bus.
                    // https://masstransit-project.com/usage/transports/rabbitmq.html#configuration
                    config.Host(
                        messageBusSettings.RabbitMq.Host,
                        "/",
                        h =>
                        {
                            h.Username(messageBusSettings.RabbitMq.Username);
                            h.Password(messageBusSettings.RabbitMq.Password);
                        });

                    // Common bus configs
                    BusConfiguratorAction(ctx, config);
                });
                return;
            }

            // Set Azure Service Bus as the message bus.
            // https://masstransit.io/documentation/configuration/transports/azure-service-bus
            busConfigurator.UsingAzureServiceBus((ctx, config) =>
            {
                config.Host(messageBusSettings.AzureServiceBus.ConnectionString);

                // Common bus configs
                BusConfiguratorAction(ctx, config);
            });
        });

        return serviceCollection;
    }

    public static IServiceCollection AddMessageBusTransactionStreamInfrastructureServices(
        this IServiceCollection serviceCollection,
        bool isProduction)
    {
        // Add services
        serviceCollection.AddScoped<IMessageBusService, MessageBusService>();

        // Add options
        serviceCollection
            .AddOptions<MessageBusOptions>()
            .BindConfiguration(MessageBusOptions.MessageBus)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Load message bus settings from the config file.
        var sp = serviceCollection.BuildServiceProvider();
        var messageBusSettings = sp.GetRequiredService<IOptions<MessageBusOptions>>().Value;

        // Add and configure the MassTransit framework.
        // https://masstransit-project.com/usage/configuration.html#configuration
        serviceCollection.AddMassTransit(busConfigurator =>
        {
            // Configures MassTransit to generate endpoint names using kebab case (e.g. submit-transaction).
            // https://masstransit-project.com/usage/containers/#bus-configuration
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            // Register consumer to handle message bus events.
            // https://masstransit-project.com/usage/configuration.html#consumers
            busConfigurator.AddConsumer<SubmitTransactionConsumer, SubmitTransactionConsumerDefinition>();

            void BusConfiguratorAction<T>(IBusRegistrationContext context, IReceiveConfigurator<T> receiveConfigurator)
                where T : IReceiveEndpointConfigurator
            {
                // Generate queue names for each receive endpoint.
                // https://masstransit-project.com/usage/guidance.html#receive-endpoints
                receiveConfigurator.ConfigureEndpoints(context);
            }

            // If environment is not production, use RabbitMQ as the message bus.
            if (!isProduction)
            {
                // Set RabbitMQ as the message bus.
                // https://masstransit-project.com/usage/transports/rabbitmq.html#rabbitmq
                busConfigurator.UsingRabbitMq((ctx, config) =>
                {
                    // Configure the message bus.
                    // https://masstransit-project.com/usage/transports/rabbitmq.html#configuration
                    config.Host(messageBusSettings.RabbitMq.Host, "/", h =>
                    {
                        h.Username(messageBusSettings.RabbitMq.Username);
                        h.Password(messageBusSettings.RabbitMq.Password);
                    });

                    // Common bus configs
                    BusConfiguratorAction(ctx, config);
                });
                return;
            }

            // Set Azure Service Bus as the message bus.
            // https://masstransit.io/documentation/configuration/transports/azure-service-bus
            busConfigurator.UsingAzureServiceBus((ctx, config) =>
            {
                config.Host(messageBusSettings.AzureServiceBus.ConnectionString);

                // Common bus configs
                BusConfiguratorAction(ctx, config);
            });
        });

        return serviceCollection;
    }

    public static IServiceCollection AddMessageBusWorkerInfrastructureServices<TDbContext>(
        this IServiceCollection serviceCollection,
        bool isProduction)
        where TDbContext : DbContext
    {
        // Add services
        serviceCollection.AddScoped<IMessageBusService, MessageBusService>();

        // Add options
        serviceCollection
            .AddOptions<MessageBusOptions>()
            .BindConfiguration(MessageBusOptions.MessageBus)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Load message bus settings from the config file.
        var sp = serviceCollection.BuildServiceProvider();
        var messageBusSettings = sp.GetRequiredService<IOptions<MessageBusOptions>>().Value;

        // Add and configure the MassTransit framework.
        // https://masstransit-project.com/usage/configuration.html#configuration
        serviceCollection.AddMassTransit(busConfigurator =>
        {
            // Configures MassTransit to generate endpoint names using kebab case (e.g. submit-transaction).
            // https://masstransit-project.com/usage/containers/#bus-configuration
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            // Configures Transactional Outbox for MassTransit.
            // https://masstransit-project.com/advanced/transactional-outbox.html
            busConfigurator.AddEntityFrameworkOutbox<TDbContext>(configurator =>
            {
                // Using default outbox configs for now, we can readjust later if needed, when we have more data to work with.
                configurator.UsePostgres();
                configurator.UseBusOutbox();
                configurator.DisableInboxCleanupService();
            });

            void BusConfiguratorAction<T>(IBusRegistrationContext context, IReceiveConfigurator<T> receiveConfigurator)
                where T : IReceiveEndpointConfigurator
            {
                // Generate queue names for each receive endpoint.
                // https://masstransit-project.com/usage/guidance.html#receive-endpoints
                receiveConfigurator.ConfigureEndpoints(context);
            }

            // If environment is not production, use RabbitMQ as the message bus.
            if (!isProduction)
            {
                // Set RabbitMQ as the message bus.
                // https://masstransit-project.com/usage/transports/rabbitmq.html#rabbitmq
                busConfigurator.UsingRabbitMq((ctx, config) =>
                {
                    // Configure the message bus.
                    // https://masstransit-project.com/usage/transports/rabbitmq.html#configuration
                    config.Host(messageBusSettings.RabbitMq.Host, "/", h =>
                    {
                        h.Username(messageBusSettings.RabbitMq.Username);
                        h.Password(messageBusSettings.RabbitMq.Password);
                    });

                    // Common bus configs
                    BusConfiguratorAction(ctx, config);
                });
                return;
            }

            // Set Azure Service Bus as the message bus.
            // https://masstransit.io/documentation/configuration/transports/azure-service-bus
            busConfigurator.UsingAzureServiceBus((ctx, config) =>
            {
                config.Host(messageBusSettings.AzureServiceBus.ConnectionString);

                // Common bus configs
                BusConfiguratorAction(ctx, config);
            });
        });

        return serviceCollection;
    }

    public static void AddMessageBusSagaInfrastructureServices(
        this IServiceCollection serviceCollection,
        bool isProduction,
        string redisConnString,
        int redisDatabaseId)
    {
        // Add options
        serviceCollection
            .AddOptions<MessageBusOptions>()
            .BindConfiguration(MessageBusOptions.MessageBus)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Load message bus settings from the config file.
        var sp = serviceCollection.BuildServiceProvider();
        var messageBusSettings = sp.GetRequiredService<IOptions<MessageBusOptions>>().Value;

        // Add and configure the MassTransit framework.
        // https://masstransit-project.com/usage/configuration.html#configuration
        serviceCollection.AddMassTransit(busConfigurator =>
        {
            // Configure MassTransit to generate endpoint names using kebab case (e.g. submit-transaction).
            // https://masstransit-project.com/usage/containers/#bus-configuration
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            // Register saga state machine for handling tx stream transactions.
            // Register a saga state machine for handling tx stream transactions.
            // https://masstransit-project.com/usage/sagas/automatonymous.html#configuration
            busConfigurator.AddSagaStateMachine<TransactionStreamSaga, TransactionStreamState>(typeof(TransactionStreamStateDefinition))

                // Configure saga state persistence using Redis key-value store.
                // https://masstransit-project.com/usage/sagas/persistence.html
                // https://masstransit-project.com/usage/sagas/redis.html
                .RedisRepository(r =>
                {
                    r.DatabaseConfiguration(redisConnString);
                    r.SelectDatabase(s => s.GetDatabase(redisDatabaseId));

                    // Optional, the default is Optimistic
                    r.ConcurrencyMode = ConcurrencyMode.Optimistic;

                    // Optional, prefix each saga instance key with the string specified
                    // Example: TransactionStreamSaga:c6cfd285-80b2-4c12-bcd3-56a00d994736
                    r.KeyPrefix = "TransactionStreamSaga";

                    // Optional, to customize the lock key
                    r.LockSuffix = "-locked";

                    // Optional, the default is 30 seconds
                    r.LockTimeout = TimeSpan.FromSeconds(90);
                });

            void BusConfiguratorAction<T>(IBusRegistrationContext registrationContext, IReceiveConfigurator<T> receiveConfigurator)
                where T : IReceiveEndpointConfigurator
            {
                // Generate queue names for each receive endpoint.
                // https://masstransit-project.com/usage/guidance.html#receive-endpoints
                receiveConfigurator.ConfigureEndpoints(registrationContext);
            }

            // If environment is not production, use RabbitMQ as the message bus.
            if (!isProduction)
            {
                // Set RabbitMQ as the message bus.
                // https://masstransit-project.com/usage/transports/rabbitmq.html#rabbitmq
                busConfigurator.UsingRabbitMq((ctx, config) =>
                {
                    // Configure the message bus.
                    // https://masstransit-project.com/usage/transports/rabbitmq.html#configuration
                    config.Host(messageBusSettings.RabbitMq.Host, "/", h =>
                    {
                        h.Username(messageBusSettings.RabbitMq.Username);
                        h.Password(messageBusSettings.RabbitMq.Password);
                    });

                    // Common bus configs
                    BusConfiguratorAction(ctx, config);
                });
                return;
            }

            // Set Azure Service Bus as the message bus.
            // https://masstransit.io/documentation/configuration/transports/azure-service-bus
            busConfigurator.UsingAzureServiceBus((ctx, config) =>
            {
                config.Host(messageBusSettings.AzureServiceBus.ConnectionString);

                // Common bus configs
                BusConfiguratorAction(ctx, config);
            });
        });
    }
}