using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WinUI_ASP.NET_Basics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        // Define an endpoint for uploading an image file
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                // Check if the file is null or empty
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                // Define the path to the uploads directory
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                
                // Create the uploads directory if it does not exist
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Define the full path for the uploaded file
                var filePath = Path.Combine(uploadsPath, file.FileName);

                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate the URL for accessing the uploaded file
                var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{file.FileName}";
                return Ok(new { Url = fileUrl });
            }
            catch (Exception ex)
            {
                // Return a bad request response with the exception message
                return BadRequest(ex.Message);
            }
        }
    }
}
