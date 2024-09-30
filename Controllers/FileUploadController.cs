using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace reusable_components.Controllers
{
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private FileUploadService _services;
        public FileUploadController(FileUploadService services)
        {
            _services = services;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
            {
                return Json(new { success = false, errorMessage = "No file selected" });
            }

            var filePath = await _services.UploadFileAsync(file, 2097152);

            if (filePath != null)
            {
                return Json(new { success = true, filePath = filePath });
            }
            else
            {
                return Json(new { success = false, ErrorMessage = "The file is too large or an error occurred during upload." });
            }



        }
    }
}
