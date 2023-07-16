using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Sample.Shared.Telemetry;

public static class OpenTelemetryExtensions
{
    public static ILoggingBuilder AddApplicationLogging(this ILoggingBuilder builder, string serviceName)
    {
        builder.ClearProviders();
        builder.AddOpenTelemetry(options =>
        {
            var resourceBuilder = ResourceBuilder
                .CreateDefault()
                .AddService(serviceName);

            options.SetResourceBuilder(resourceBuilder);

            options
                .AddConsoleExporter();
        });

        return builder;
    }

    public static IServiceCollection AddApplicationTelemetry(this IServiceCollection services, string serviceName)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(builder =>
            {
                builder.AddService(serviceName);
            })
            .WithTracing(builder =>
            {
                builder
                    .AddSource(TelemetryConstants.AppSourceName)
                    .AddSource("MassTransit.RabbitMQ")
                    // this line enables tracing in masstransit https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/326#issuecomment-1120637599
                    .AddSource("MassTransit")
                    .AddAspNetCoreInstrumentation(o =>
                    {
                        o.RecordException = true;
                    })
                    .AddHttpClientInstrumentation(o =>
                    {
                        o.RecordException = true;
                    })
                    //.AddMassTransitInstrumentation()
                    .AddJaegerExporter();
            })
            .WithMetrics(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        services.Configure<OpenTelemetryLoggerOptions>(opt =>
        {
            opt.IncludeScopes = true;
            opt.ParseStateValues = true;
            opt.IncludeFormattedMessage = true;
        });

        return services;
    }
}