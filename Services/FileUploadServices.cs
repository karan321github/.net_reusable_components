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
            // Check file size
            if (file.Length > maxFileSizeInBytes)
            {
                return null; // File is too large
            }

            
            var baseFolderPath = _environment.WebRootPath ?? _environment.ContentRootPath;
            if (baseFolderPath == null)
            {
                _logger.LogError("Both WebRootPath and ContentRootPath are null.");
                throw new ArgumentNullException("WebRootPath", "Both WebRootPath and ContentRootPath are null.");
            }

            var uploadsFolderPath = Path.Combine(baseFolderPath, "uploads");

            // Ensure the uploads directory exists
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Build the file path for saving the file
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            // Save the file to the local file system
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file.");
            return null;
        }
    }
}
