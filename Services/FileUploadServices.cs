public class FileUploadService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileUploadService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _environment = environment;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> UploadFileAsync(IFormFile file, int maxFileSizeInBytes)
    {
        try
        {
           
            if (file.Length > maxFileSizeInBytes)
            {
                return null; 
            }

            
            var baseFolderPath = _environment.WebRootPath ?? _environment.ContentRootPath;
            if (baseFolderPath == null)
            {
                _logger.LogError("Both WebRootPath and ContentRootPath are null.");
                throw new ArgumentNullException("WebRootPath", "Both WebRootPath and ContentRootPath are null.");
            }

            var uploadsFolderPath = Path.Combine(baseFolderPath, "uploads");

            
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Build the file path for saving the file
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

<<<<<<< HEAD
            // Save the file to the local file system
=======
            
>>>>>>> 3dbb94e73b0632ed98e9cd3f6cdf84b1bbd9f5f3
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

<<<<<<< HEAD
            // Access HttpContext via IHttpContextAccessor
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogError("HttpContext is null.");
                return null;
            }

            // Create the public URL to access the file
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
            //var relativeFilePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");
            var fileUrl = $"{baseUrl}/api/FileUpload/preview/{uniqueFileName}";

            // Return the public file URL
            return fileUrl;
=======
           
            var relativeFilePath = Path.Combine("uploads", uniqueFileName);
            return relativeFilePath; 
>>>>>>> 3dbb94e73b0632ed98e9cd3f6cdf84b1bbd9f5f3
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file.");
            return null;
        }
    }
}
