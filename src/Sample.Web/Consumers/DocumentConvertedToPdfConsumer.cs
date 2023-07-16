using MassTransit;
using Sample.Shared.ConvertToPdf;
using Sample.Web.ProcessingTask;

namespace Sample.Web.Consumers;

public class DocumentConvertedToPdfConsumer : IConsumer<FileConvertedToPdf>
{
    private readonly ITasksService _tasksService;
    private readonly ILogger<DocumentConvertedToPdfConsumer> _logger;

    public DocumentConvertedToPdfConsumer(ITasksService tasksService, ILogger<DocumentConvertedToPdfConsumer> logger)
    {
        _tasksService = tasksService;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<FileConvertedToPdf> context)
    {
        _logger.LogInformation("Task {Id} converted to PDF. Competing the task.", context.Message.Id);
        _tasksService.CompleteTask(context.Message.Id);

        return Task.CompletedTask;
    }
}
