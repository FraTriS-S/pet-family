using CSharpFunctionalExtensions;
using PetFamily.Application.Files.Upload;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers.FileProvider;

public interface IFileProvider
{
    Task<Result<string, Error>> PresignedGetFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFilesAsync(IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);
    Task<Result<string, Error>> RemoveFileAsync(string fileName, CancellationToken cancellationToken = default);
}