using fs.Presentation.Results;
using FS.Contracts;
using FS.Core.Abstractions.Common;
using FS.Core.Features;
using Microsoft.AspNetCore.Mvc;

namespace fs.Presentation.Controllers;

[Route("api/upload/files")]
[ApiController]
public class SimpleUploadController : ControllerBase
{
    /// <summary>
    /// Шаг 1. Регистрирует asset в состоянии ожидания загрузки и выдаёт presigned PUT URL.
    /// Файл в теле запроса не принимается.
    /// </summary>
    [HttpPost("init")]
    public async Task<IActionResult> Init(
        [FromBody] InitUploadRequest request,
        [FromServices] ICommandHandler<InitUploadResponse, InitUploadCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new InitUploadCommand(request), cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>
    /// Шаг 3. Подтверждает загрузку: сверяет фактические метаданные объекта в storage
    /// и переводит asset в READY (или возвращает доменную ошибку).
    /// </summary>
    [HttpPost("{mediaAssetId:guid}/complete")]
    public async Task<IActionResult> Complete(
        [FromRoute] Guid mediaAssetId,
        [FromServices] ICommandHandler<CompleteUploadResponse, CompleteUploadCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new CompleteUploadCommand(new CompleteUploadRequest(mediaAssetId)), cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>
    /// Минимальная проверка получения файла: для READY asset'а отдаёт свежий presigned GET URL.
    /// </summary>
    [HttpGet("{mediaAssetId:guid}/url")]
    public async Task<IActionResult> GetUrl(
        [FromRoute] Guid mediaAssetId,
        [FromServices] IQueryHandler<GetMediaAssetUrlResponse, GetMediaAssetUrlQuery> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetMediaAssetUrlQuery(mediaAssetId), cancellationToken);

        return result.ToActionResult();
    }
}
