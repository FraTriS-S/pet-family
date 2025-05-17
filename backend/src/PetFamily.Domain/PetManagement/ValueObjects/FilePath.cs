using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record FilePath
{
    private static readonly string[] AllowedExtensions = ["jpg", "jpeg", "png", "bmp"];

    private FilePath(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        if (path == Guid.Empty)
        {
            return Errors.General.ValueIsInvalid(nameof(path));
        }

        if (string.IsNullOrWhiteSpace(extension) && !AllowedExtensions.Contains(extension))
        {
            return Errors.General.ValueIsInvalid(nameof(extension));
        }

        var filePath = path + extension;

        return new FilePath(filePath);
    }
}