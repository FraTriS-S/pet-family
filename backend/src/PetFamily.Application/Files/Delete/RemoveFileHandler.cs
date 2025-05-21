using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Delete;

public class RemoveFileHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<RemoveFileHandler> _logger;

    public RemoveFileHandler(IFileProvider fileProvider, ILogger<RemoveFileHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<List<string>, ErrorList>> HandleAsync(
        IEnumerable<string> fileNames, CancellationToken cancellationToken = default)
    {
        var result = await _fileProvider.RemoveFilesAsync(fileNames, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        _logger.LogInformation("Files were removed");

        return result.Value.ToList();
    }
}