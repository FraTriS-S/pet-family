using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Files.Upload;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider(ILogger<MinioProvider> logger, IMinioClient minioClient)
    : IFileProvider
{
    private readonly ILogger<MinioProvider> _logger = logger;
    private readonly IMinioClient _minioClient = minioClient;

    private const string BUCKET_NAME = "photos";
    private const int ONE_DAY_EXPIRY = 24 * 3600;

    public async Task<Result<string, Error>> PresignedGetFileAsync(
        string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName);

            var statObject = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);

            if (statObject.ContentType is null)
            {
                return Error.NotFound("file.not.found", $"file with name: {fileName} not found");
            }

            var args = new PresignedGetObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName)
                .WithExpiry(ONE_DAY_EXPIRY);

            var fileUrl = await _minioClient.PresignedGetObjectAsync(args);

            return fileUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to presigned get file in minio");
            return Error.Failure("file.get", "Failed to presigned get file in minio");
        }
    }

    public async Task<Result<string, Error>> UploadFileAsync(
        UploadFileCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var isBucketExistsArgs = new BucketExistsArgs()
                .WithBucket(BUCKET_NAME);

            var isBucketExists = await _minioClient.BucketExistsAsync(isBucketExistsArgs, cancellationToken);

            if (isBucketExists == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(command.BucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(command.BucketName)
                .WithObjectSize(command.Stream.Length)
                .WithStreamData(command.Stream)
                .WithObject($"{command.FileName}.pdf");

            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return result.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file in minio");
            return Error.Failure("file.upload", "Failed to upload file in minio");
        }
    }

    public async Task<Result<string, Error>> RemoveFileAsync(
        string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName);

            var statObject = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);

            if (statObject.ContentType is null)
            {
                return Error.NotFound("file.not.found", $"file with name: {fileName} not found");
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove file in minio");
            return Error.Failure("file.remove", "Failed to remove file in minio");
        }
    }
}