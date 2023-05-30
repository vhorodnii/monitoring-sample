using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sample.Shared;

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
                    .AddSource(TelemetryConstants.AppSourceName)
                    .AddAspNetCoreInstrumentation(o =>
                    {
                        o.EnrichWithHttpResponse = (activity, resp) =>
                        {
                            resp.Headers.CorrelationContext = activity.TraceId.ToString();
                        };
                        o.RecordException = true;
                    })
                    .AddHttpClientInstrumentation(o =>
                    {
                        o.RecordException = true;
                    })
                    .AddJaegerExporter();
            })
            .WithMetrics(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        return services;
    }
}