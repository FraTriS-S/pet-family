using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer.Pet.ValueObjects;

public record Color
{
    private Color(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Color> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Color>("Color is required");
        }

        return new Color(value);
    }
}