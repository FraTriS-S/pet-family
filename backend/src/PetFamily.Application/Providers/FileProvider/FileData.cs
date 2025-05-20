using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.Providers.FileProvider;

public record FileData(Stream Stream, FilePath FilePath, string BucketName);