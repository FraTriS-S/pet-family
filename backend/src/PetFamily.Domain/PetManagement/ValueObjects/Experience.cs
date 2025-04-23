using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Experience
{
    private const int MIN_VALUE = 0;

    private Experience(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<Experience, Error> Create(int value)
    {
        if (MIN_VALUE > value)
        {
            return Errors.General.ValueIsInvalid(nameof(Experience));
        }

        return new Experience(value);
    }
}