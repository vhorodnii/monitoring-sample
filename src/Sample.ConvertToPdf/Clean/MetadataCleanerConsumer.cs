using MassTransit;
using Sample.Shared.Clean;

namespace Sample.ConvertToPdf.Clean
{
    public class MetadataCleanerConsumer : IConsumer<CleanDocument>
    {
        private readonly ILogger<MetadataCleanerConsumer> _logger;

        public MetadataCleanerConsumer(ILogger<MetadataCleanerConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CleanDocument> context)
        {
            _logger.LogInformation("Cleaning metadata in {File}.", context.Message.Path);

            await context.Publish(new DocumentCleaned
            {
                Id = context.Message.Path,
            });

            _logger.LogInformation("Metadata lceaned in {File}.", context.Message.Path);
        }
    }
}
