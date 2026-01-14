using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using teleport.Models;

namespace teleport.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class FileController : ControllerBase
    {
        private readonly FileService _fs;

        public FileController(FileService fs)
        {
            _fs = fs;
        }

        [HttpPost("uploads")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file selected");

            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var OriginalFileName = Path.GetFileName(file.FileName);
            var FileExt = Path.GetExtension(file.FileName);
            var NewName = Guid.NewGuid().ToString() + FileExt;
            var FilePath = Path.Combine(uploadDir, NewName);

            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var token = TokenHelper.Generate(5);

            var fileModel = new FileModel
            {
                Token = token,
                StoredName = NewName,
                OriginalName = OriginalFileName,
                IsDownloaded = false
            };

            _fs.FileUpload(fileModel);

            return Ok(token);
        }

        [HttpGet("download/{token}")]
        public IActionResult Download(string token)
        {
            var file = _fs.GetByToken(token);

            if (file == null)
                return BadRequest("Invalid link");

            if (file.IsDownloaded)
                return BadRequest("File is already downloaded.");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", file.StoredName);

            if (!System.IO.File.Exists(path))
                return NotFound("File not found on server");

            var bytes = System.IO.File.ReadAllBytes(path);

            _fs.MarkDownloaded(file.Id);

            System.IO.File.Delete(path);

            return File(bytes, "application/octet-stream", file.OriginalName);

        }
    }
}