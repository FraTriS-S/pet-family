using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.PresignedGet;
using PetFamily.Application.Files.Upload;

namespace PetFamily.API.Controllers.Files;

public class FileController : ApplicationController
{
    private const string BUCKET_NAME = "photos";

    [HttpGet("{fileName}")]
    public async Task<IActionResult> PresignedGetFile(
        [FromServices] PresignedGetFileHandler handler,
        [FromRoute] string fileName,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(fileName, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(
        [FromServices] UploadFileHandler handler,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        var objectName = Guid.NewGuid().ToString();

        var command = new UploadFileCommand(stream, BUCKET_NAME, objectName);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> RemoveFile(
        [FromServices] RemoveFileHandler handler,
        [FromRoute] string fileName,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(fileName, cancellationToken);

        return result.ToResponse();
    }
}