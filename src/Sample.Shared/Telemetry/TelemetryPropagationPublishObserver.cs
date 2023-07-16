using MassTransit;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Sample.Shared.Telemetry
{
    public class TelemetryPropagationPublishObserver : IPublishObserver
    {
        private readonly Tracer _tracer;

        public TelemetryPropagationPublishObserver(TracerProvider tracerProvider)
        {
            _tracer = tracerProvider.GetTracer("MassTransit.RabbitMQ");
        }

        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            var cut = Activity.Current;
            //using var span = _tracer.StartActiveSpan("RabbitMQ.Publishing");
            //span.Context.TraceId
            //context.CorrelationId = span.Context.SpanId.;

            return Task.CompletedTask;
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
