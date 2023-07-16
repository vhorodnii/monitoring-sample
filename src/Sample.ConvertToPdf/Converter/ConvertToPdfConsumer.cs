using MassTransit;
using Sample.Shared.ConvertToPdf;
using Sample.Shared.FileStorage;

namespace Sample.ConvertToPdf.Converter
{
    public class ConvertToPdfConsumer : IConsumer<ConvertToPdfFile>
    {
        private readonly IFileStorage _storage;
        private readonly ILogger<ConvertToPdfConsumer> _logger;

        public ConvertToPdfConsumer(IFileStorage storage, ILogger<ConvertToPdfConsumer> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ConvertToPdfFile> context)
        {
            _logger.LogInformation("Converting {File} to PDF.", context.Message.Path);

            await context.Publish(new FileConvertedToPdf
            {
                Id = context.Message.Path,
            });

            _logger.LogInformation("File {File} converted to PDF.", context.Message.Path);
        }
    }
}
