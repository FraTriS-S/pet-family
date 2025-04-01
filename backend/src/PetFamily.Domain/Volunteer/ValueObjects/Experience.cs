using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer.ValueObjects;

public record Experience
{
    private Experience(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<Experience> FromValue(int value)
    {
        if (value < 0)
        {
            return Result.Failure<Experience>("Experience must be greater than 0");
        }

        return new Experience(value);
    }
}