using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ServicesAbstractions;

namespace Services.UploadFiles
{
    public class FileService(IWebHostEnvironment _webHostEnvironment) : IFileService
    {
     

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
           if(file == null || file.Length == 0) throw new ArgumentException("File is empty or null.");
            var webRootPath = _webHostEnvironment.WebRootPath
            ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var uploadsFolder = Path.Combine(webRootPath, "uploads",folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder,uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{folderName}/{uniqueFileName}";
        }


        public bool DeleteFile(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return false;
            var webRootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var filePath = Path.Combine(webRootPath, fileUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}
