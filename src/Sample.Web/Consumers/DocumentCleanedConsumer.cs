using MassTransit;
using Sample.Shared.Clean;
using Sample.Shared.ConvertToPdf;
using Sample.Web.ProcessingTask;

namespace Sample.Web.Consumers
{
    public class DocumentCleanedConsumer : IConsumer<DocumentCleaned>
    {
        private readonly ITasksService _tasksService;
        private readonly IBus _bus;
        private readonly ILogger<DocumentCleanedConsumer> _logger;

        public DocumentCleanedConsumer(ITasksService tasksService, IBus bus, ILogger<DocumentCleanedConsumer> logger)
        {
            _tasksService = tasksService;
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DocumentCleaned> context)
        {
            var cutr = System.Diagnostics.Activity.Current;
            _logger.LogInformation("Task {Id} cleaned metadata. Deciding the next step.", context.Message.Id);
            if (_tasksService.NeedConverting(context.Message.Id))
            {
                await _bus.Publish(new ConvertToPdfFile
                {
                    Path = context.Message.Id
                });
                _logger.LogInformation("Task {Id} was sent to converting PDF.", context.Message.Id);
            }
            else
            {
                _logger.LogInformation("Task {Id} completed. No futher steps. Competing the task.", context.Message.Id);
                _tasksService.CompleteTask(context.Message.Id);
            }
        }
    }
}
