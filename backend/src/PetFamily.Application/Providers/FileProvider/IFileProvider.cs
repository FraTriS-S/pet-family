using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers.FileProvider;

public interface IFileProvider
{
    Task<Result<string, Error>> PresignedGetFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFilesAsync(IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<string>, Error>> RemoveFilesAsync(IEnumerable<string> filesNames, CancellationToken cancellationToken = default);
}