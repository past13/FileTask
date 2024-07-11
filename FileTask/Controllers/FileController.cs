using FileTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileTask.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController(IFileService _fileService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { error = "No selected file" });
        }

        try
        {
            var stream = file.OpenReadStream();
            
            var invalidLines = await _fileService.ValidateAccountsAsync(stream, (elapsed, line) =>
            {
                Console.WriteLine($"Validation line number {line} took: {elapsed}");
            });

            if (invalidLines.Count != 0)
            {
                return BadRequest(new
                {
                    fileValid = false,
                    invalidLines
                });
            }

            return Ok(new { fileValid = true });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }
}