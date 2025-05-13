using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.PresignedGet;

public class PresignedGetFileHandler(
    IFileProvider fileProvider,
    ILogger<PresignedGetFileHandler> logger)
{
    private readonly IFileProvider _fileProvider = fileProvider;
    private readonly ILogger<PresignedGetFileHandler> _logger = logger;

    public async Task<Result<string, ErrorList>> HandleAsync(
        string fileName, CancellationToken cancellationToken = default)
    {
        var result = await _fileProvider.PresignedGetFileAsync(fileName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        _logger.LogInformation("File with name:{fileName} was successfully got", fileName);
        return result.Value;
    }
}