using CSharpFunctionalExtensions;
using PetFamily.Application.Files.Upload;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> PresignedGetFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<Result<string, Error>> UploadFileAsync(UploadFileCommand command, CancellationToken cancellationToken = default);
    Task<Result<string, Error>> RemoveFileAsync(string fileName, CancellationToken cancellationToken = default);
}