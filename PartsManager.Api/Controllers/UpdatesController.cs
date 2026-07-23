using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PartsManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UpdatesController : ControllerBase
{
    [HttpGet("version")]
    public async Task<IActionResult> GetVersion()
    {
        var versionFilePath = Path.Combine(AppContext.BaseDirectory, "Updates", "version.txt");
        if (!System.IO.File.Exists(versionFilePath))
        {
            versionFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Updates", "version.txt");
        }

        if (!System.IO.File.Exists(versionFilePath))
        {
            return NotFound("version.txt not found on server.");
        }

        var versionText = await System.IO.File.ReadAllTextAsync(versionFilePath);
        return Ok(versionText.Trim());
    }

    [HttpGet("download")]
    public IActionResult DownloadClientZip()
    {
        var zipFilePath = Path.Combine(AppContext.BaseDirectory, "Updates", "Client.zip");
        if (!System.IO.File.Exists(zipFilePath))
        {
            zipFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Updates", "Client.zip");
        }

        if (!System.IO.File.Exists(zipFilePath))
        {
            return NotFound("Client.zip not found on server.");
        }

        var fileStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read);
        return File(fileStream, "application/zip", "Client.zip");
    }
}
