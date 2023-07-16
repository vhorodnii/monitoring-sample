using MassTransit;
using Sample.Shared.Clean;
using Sample.Shared.ConvertToPdf;
using Sample.Shared.ProcessingTask;
using Sample.Web.ProcessingTask;

namespace Sample.Web.Consumers;

public class NewTaskReceivedConsumer : IConsumer<NewTaskRequest>
{
    private readonly ITasksService _tasksService;
    private readonly IBus _bus;
    private readonly ILogger<NewTaskRequest> _logger;

    public NewTaskReceivedConsumer(ITasksService tasksService, IBus bus, ILogger<NewTaskRequest> logger)
    {
        _tasksService = tasksService;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<NewTaskRequest> context)
    {
        var task = context.Message;

        _tasksService.StartNewTask(task);
        _logger.LogInformation("New task {Id} started.", context.Message.Id);

        if (task.CleanDocument)
        {
            await _bus.Publish(new CleanDocument
            {
                Path = task.Id
            });
        }
        else if (task.ConvertToPdf)
        {
            await _bus.Publish(new ConvertToPdfFile
            {
                Path = task.Id
            });
        }
        else
        {
            _tasksService.CompleteTask(task.Id);
        }
    }
}
