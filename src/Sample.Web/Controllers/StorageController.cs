using Microsoft.AspNetCore.Mvc;
using Sample.Shared.FileStorage;

namespace Sample.Web.Controllers
{
    [Route("api/storage")]
    public class StorageController : ControllerBase
    {
        private readonly IFileStorage _storage;

        public StorageController(IFileStorage storage)
        {
            _storage = storage;
        }

        [Route("save")]
        [HttpPost]
        public IActionResult SaveFile(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            var id = $"task-{Guid.NewGuid()}{ext}";
            using var stream = file.OpenReadStream();
            _storage.SaveFile(id, stream);
            return Ok(id);
        }
    }
}
