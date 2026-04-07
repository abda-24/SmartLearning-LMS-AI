using Microsoft.AspNetCore.Http;
namespace ServicesAbstractions
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        bool DeleteFile(string fileUrl);
    }
}
