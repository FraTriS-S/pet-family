using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Description
{
    public const int MAX_LENGTH = 2000;

    private Description(string? value)
    {
        Value = value;
    }

    public string? Value { get; }

    public static Result<Description, Error> Create(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Length > MAX_LENGTH)
        {
            return Errors.General.ValueLengthIsInvalid(nameof(Description));
        }

        return new Description(value);
    }
}