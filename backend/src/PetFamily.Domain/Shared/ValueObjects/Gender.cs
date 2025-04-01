using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Gender
{
    private Gender(Enums.Gender value)
    {
        Value = value;
    }

    public Enums.Gender Value { get; }

    public static Result<Gender> Create(Enums.Gender value)
    {
        return new Gender(value);
    }
}