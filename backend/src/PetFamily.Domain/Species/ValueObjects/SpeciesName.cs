using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species.ValueObjects;

public record SpeciesName
{
    public const int MAX_LENGTH = 100;

    private SpeciesName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<SpeciesName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
        {
            return Errors.General.ValueIsInvalid(nameof(value));
        }

        return new SpeciesName(value);
    }
}