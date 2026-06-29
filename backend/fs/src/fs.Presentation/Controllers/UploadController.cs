using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Features;
using Microsoft.AspNetCore.Mvc;

namespace fs.Presentation.Controllers;

[Route("api/upload")]
[ApiController]
public class UploadController : ControllerBase
{
    [HttpPost("files/start-multipart")]
    public async Task<IActionResult> StartUploadMultipartFile(
        [FromBody] StartMultipartUploadRequest request,
        [FromServices] StartMultipartUploadHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new StartMultipartUploadCommand(request), cancellationToken);

        return Ok(response.Value);
    }

    [HttpPost("files/complete-multipart")]
    public async Task<IActionResult> CompleteUploadMultipartFile(
       [FromBody] CompleteMultipartUploadRequest request,
       [FromServices] CompleteMultipartUploadHandler handler,
       CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new CompleteMultipartUploadCommand(request), cancellationToken);

        return Ok(response.Value);
    }

    [HttpPost("files")]
    public async Task<IActionResult> UploadFile(
        [FromForm] IFormFile file, [FromServices] IS3Provider s3Provider, CancellationToken cancellationToken)
    {

        await s3Provider.UploadFileAsync(
            file.OpenReadStream(), "images", $"raw/{Guid.CreateVersion7()}", file.ContentType, cancellationToken);

        return Ok();
    }
}
