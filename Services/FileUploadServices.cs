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
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

           
            var relativeFilePath = Path.Combine("uploads", uniqueFileName);
            return relativeFilePath; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file.");
            return null;
        }
    }
}
