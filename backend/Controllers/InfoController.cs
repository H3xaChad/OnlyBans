using Microsoft.AspNetCore.Mvc;
using OnlyBans.Backend.Models;

namespace OnlyBans.Backend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class InfoController : ControllerBase {
    
    [HttpPost("supported-image-types", Name = "getSupportedImageTypes")]
    public IActionResult GetSupportedImageTypes() {
        var supportedImageTypes = ImageTypeExtensions.GetFileExtensions();
        return Ok(supportedImageTypes);
    }
}