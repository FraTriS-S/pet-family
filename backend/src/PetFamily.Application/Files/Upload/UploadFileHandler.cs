using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Upload;

public class UploadFileHandler(IFileProvider fileProvider, ILogger<UploadFileHandler> logger)
{
    private readonly IFileProvider _fileProvider = fileProvider;
    private readonly ILogger<UploadFileHandler> _logger = logger;

    public async Task<Result<string, ErrorList>> HandleAsync(
        UploadFileCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _fileProvider.UploadFileAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        _logger.LogInformation(
            "Upload file with name:{command.FileName} completed", command.FileName);

        return result.Value;
    }
}