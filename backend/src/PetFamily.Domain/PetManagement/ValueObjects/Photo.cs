namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Photo
{
    public Photo(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public FilePath PathToStorage { get; }
}