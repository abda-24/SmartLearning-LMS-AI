using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presantion.Controllers
{
    [Authorize]
    public class FilesController(IFileService _fileService):BaseController
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string folderName = "general")
        {
            try
            {
                var fileUrl = await _fileService.UploadFileAsync(file, folderName);
                return Ok(new { url = fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("delete")]
        public IActionResult DeleteFile([FromQuery] string fileUrl)
        {
            var isDeleted = _fileService.DeleteFile(fileUrl);

            if (!isDeleted)
                return NotFound(new { message = "File not found or already deleted." });

            return Ok(new { message = "File deleted successfully." });
        }

    }
}
