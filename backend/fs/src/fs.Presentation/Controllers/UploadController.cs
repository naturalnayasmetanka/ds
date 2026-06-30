using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Features;
using Microsoft.AspNetCore.Mvc;

namespace fs.Presentation.Controllers;

[Route("api/upload")]
[ApiController]
public class UploadController : ControllerBase
{
    [HttpPost("files/multipart")]
    public async Task<IActionResult> UploadMultipartFile(
        [FromBody] StartMultipartUploadRequest request,
        [FromServices] StartMultipartUploadHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new StartMultipartUploadCommand(request), cancellationToken);

        return Ok();
    }


    [HttpPost("files")]
    public async Task<IActionResult> UploadFile(
        [FromForm] IFormFile file, [FromServices] IS3Provider s3Provider, CancellationToken cancellationToken)
    {

        await s3Provider.UploadFileAsync(
            file.OpenReadStream(), "images", $"raw/{Guid.CreateVersion7()}", file.ContentType, cancellationToken);

        return Ok();
    }

    [HttpGet("files/url")]
    public async Task<IActionResult> DawnloadFile(
       string bucket, string key, [FromServices] IS3Provider s3Provider, CancellationToken cancellationToken)
    {

        var result = await s3Provider.GenerateDownloadUrl(bucket, key);

        return Ok(result);
    }

    [HttpPut("files/url")]
    public async Task<IActionResult> UploadFile(
       string bucket, string key, [FromServices] IS3Provider s3Provider, CancellationToken cancellationToken)
    {

        var result = await s3Provider.GenerateUploadUrlAsync(bucket, key);

        return Ok(result);
    }
}
