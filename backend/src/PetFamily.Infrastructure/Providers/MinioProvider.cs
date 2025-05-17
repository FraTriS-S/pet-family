using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers.FileProvider;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider(ILogger<MinioProvider> logger, IMinioClient minioClient)
    : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
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

    public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFilesAsync(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesData.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(filesList, cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var results = pathsResult.Select(p => p.Value).ToList();

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);

            return Error.Failure("file.upload", "Fail to upload files in minio");
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

    private async Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FilePath.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FilePath.Path,
                fileData.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..filesData.Select(file => file.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient
                .BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}