using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace reusable_components.Controllers
{
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private readonly FileUploadService _services;
        private readonly IWebHostEnvironment _environment;

        public FileUploadController(FileUploadService services, IWebHostEnvironment environment)
        {
            _services = services;
            _environment = environment;
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadFile(IFormFile file)
        //{
        //    if (file == null)
        //    {
        //        return Json(new { success = false, errorMessage = "No file selected" });
        //    }

        //    var fileUrl = await _services.UploadFileAsync(file, 2097152);

        //    if (fileUrl != null)
        //    {
        //        return Json(new { success = true, fileUrl = fileUrl });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, ErrorMessage = "The file is too large or an error occurred during upload." });
        //    }
        //}





        [HttpPost]
        public async Task<IActionResult> onfileUpload(IFormFile file)
        {
            if(file == null)
            {
                return Json(new { success = false, errorMessage = "No file is uploaded" });
            }

            var uploadedFileUrl = await _services.UploadFileAsync(file , 2097152);


     
            if(uploadedFileUrl != null)
            {
                return Json(new { success = true, file = file });
            } else
            {
                return Json(new { success = false, errorMessgae = "File is too large" });
            }
        }


        [HttpGet("preview/{fileName}")]
        public IActionResult GetImagePreview(string fileUrl)
        {
            var baseFolderPath = _environment.WebRootPath ?? _environment.ContentRootPath;
            if(baseFolderPath == null)
            {
                return NotFound("Server configuration error");
            }

            var cPath = Path.Combine(baseFolderPath,"uploads", fileUrl);

            if (!System.IO.File.Exists(cPath))
            {
                return NotFound("Image Not found");
            }

            var image = System.IO.File.OpenRead(cPath);
            return File(image, "image/jpeg");
        }













        //[HttpGet("preview/{fileName}")]
        //public IActionResult GetImagePreview(string fileName)
        //{
        //    var baseFolderPath = _environment.WebRootPath ?? _environment.ContentRootPath;
        //    if (baseFolderPath == null)
        //    {
        //        return NotFound("Server configuration error");
        //    }

        //    var filePath = Path.Combine(baseFolderPath, "uploads", fileName);

        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return NotFound("Image not found");
        //    }

        //    var image = System.IO.File.OpenRead(filePath);
        //    return File(image, "image/jpeg"); 
        //}
    }
}