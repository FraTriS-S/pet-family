using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Delete;

public class RemoveFileHandler(IFileProvider fileProvider, ILogger<RemoveFileHandler> logger)
{
    private readonly IFileProvider _fileProvider = fileProvider;
    private readonly ILogger<RemoveFileHandler> _logger = logger;

    public async Task<Result<string, ErrorList>> HandleAsync(
        string fileName, CancellationToken cancellationToken = default)
    {
        var result = await _fileProvider.RemoveFileAsync(fileName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        _logger.LogInformation("File with name:{fileName} was removed", fileName);
        return result.Value;
    }
}