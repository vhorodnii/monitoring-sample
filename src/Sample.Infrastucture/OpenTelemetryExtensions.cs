using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.Logging;

public static class OpenTelemetryExtensions
{
    public static ILoggingBuilder AddApplicationLogging(this ILoggingBuilder builder, string serviceName)
    {
        builder.ClearProviders();
        builder.AddOpenTelemetry(options =>
        {
            var resourceBuilder = ResourceBuilder.CreateDefault();
            resourceBuilder.AddService(serviceName);
            options.SetResourceBuilder(resourceBuilder);

            options.AddConsoleExporter();
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
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation(o =>
                    {
                        o.RecordException = true;
                    })
                    .AddConsoleExporter();
            })
            .WithMetrics(builder =>
            {
                builder.AddAspNetCoreInstrumentation()
                    .AddConsoleExporter();
            });

        return services;
    }
}