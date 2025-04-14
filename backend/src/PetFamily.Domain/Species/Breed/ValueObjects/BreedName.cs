using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species.Breed.ValueObjects;

public record BreedName
{
    public const int MAX_LENGTH = 100;

    private BreedName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<BreedName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
        {
            return Errors.General.ValueIsInvalid(nameof(value));
        }

        return new BreedName(value);
    }
}