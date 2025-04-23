using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Height
{
    private const int MIN_VALUE = 0;

    private Height(float value)
    {
        Value = value;
    }

    public float Value { get; }

    public static Result<Height, Error> Create(float value)
    {
        if (value < MIN_VALUE)
        {
            return Errors.General.ValueIsInvalid(nameof(value));
        }

        return new Height(value);
    }
}