using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Shared.ConvertToPdf;
using Sample.Shared.FileStorage;

namespace Sample.Web.Controllers
{
    [Route("api/processor")]
    public class ProcessorController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IFileStorage _storage;

        public ProcessorController(IBus bus, IFileStorage storage)
        {
            _bus = bus;
            _storage = storage;
        }

        [Route("")]
        [HttpGet]
        public IActionResult SendEvent(string value)
        {
            _bus.Publish(new ConvertToPdfFile
            {
                Path = value
            });
            return Ok(value);
        }

        [HttpPost]
        public IActionResult ProcessDocument([FromBody] ProcessingOptions processingOptions)
        {
            if (!_storage.FilePresent(processingOptions.File))
            {
                return BadRequest("File not found");
            }

            if (processingOptions.ConvertToPdf)
            {
                _bus.Publish(new ConvertToPdfFile
                {
                    Path = processingOptions.File
                });
            }

            return Ok();
        }

        [Route("save")]
        [HttpPost]
        public IActionResult SaveFile(IFormFile file)
        {
            var filename = $"file-{Guid.NewGuid()}";
            _storage.SaveFile(filename, file.OpenReadStream());
            return Ok(filename);
        }
    }
}
