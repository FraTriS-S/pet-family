using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer.ValueObjects;

public record Email
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
        {
            return Result.Failure<Email>("Email is required");
        }

        return new Email(value);
    }
}