using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Shared.FileStorage;
using Sample.Shared.ProcessingTask;

namespace Sample.Web.Controllers
{
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IFileStorage _storage;

        public TasksController(IBus bus, IFileStorage fileStorage)
        {
            _bus = bus;
            _storage = fileStorage;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateProcessingTaskAsync([FromBody] NewTaskRequest processingOptions)
        {
            if (!_storage.FilePresent(processingOptions.Id))
            {
                return BadRequest("File not found");
            }

            await _bus.Publish(new NewTaskRequest
            {
                Id = processingOptions.Id,
                CleanDocument = processingOptions.CleanDocument,
                ConvertToPdf = processingOptions.ConvertToPdf,
            });

            return Ok();
        }
    }
}
