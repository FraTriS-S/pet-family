using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.PresignedGet;
using PetFamily.Application.Files.Upload;

namespace PetFamily.API.Controllers.Files;

public class FileController : ApplicationController
{
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
    public async Task<IActionResult> UploadFiles(
        [FromServices] UploadFileHandler handler,
        IFormFileCollection files,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();

        var fileDtos = fileProcessor.Process(files);

        var command = new UploadFilesCommand(fileDtos);

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