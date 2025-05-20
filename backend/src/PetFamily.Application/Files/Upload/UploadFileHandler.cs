using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers.FileProvider;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Upload;

public class UploadFileHandler(IFileProvider fileProvider, ILogger<UploadFileHandler> logger)
{
    private readonly IFileProvider _fileProvider = fileProvider;
    private readonly ILogger<UploadFileHandler> _logger = logger;

    private const string BUCKET_NAME = "photos";

    public async Task<Result<List<string>, ErrorList>> HandleAsync(
        UploadFilesCommand command, CancellationToken cancellationToken = default)
    {
        List<FileData> filesData = [];

        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
            {
                return filePath.Error.ToErrorList();
            }

            var fileContent = new FileData(file.Content, filePath.Value, BUCKET_NAME);

            filesData.Add(fileContent);
        }

        var uploadResult = await _fileProvider.UploadFilesAsync(filesData, cancellationToken);

        if (uploadResult.IsFailure)
        {
            return uploadResult.Error.ToErrorList();
        }

        _logger.LogInformation("Files upload completed");

        var filesPaths = filesData.Select(x => x.FilePath.Path);

        return filesPaths.ToList();
    }
}