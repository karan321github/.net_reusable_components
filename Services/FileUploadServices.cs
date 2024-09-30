public class FileUploadService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileUploadService> _logger;

    public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger)
    {
        _environment = environment;
        _logger = logger;
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

            // Use WebRootPath or ContentRootPath for the file save path
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
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the relative file path to be used for preview
            var relativeFilePath = Path.Combine("uploads", uniqueFileName);
            return relativeFilePath; // Return only the relative path
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file.");
            return null;
        }
    }
}
