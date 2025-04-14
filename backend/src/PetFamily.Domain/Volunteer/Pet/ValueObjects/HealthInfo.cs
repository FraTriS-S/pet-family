using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer.Pet.ValueObjects;

public record HealthInfo
{
    public const int MAX_LENGTH = 2000;

    private HealthInfo(string? value)
    {
        Value = value;
    }

    public string? Value { get; }

    public static Result<HealthInfo, Error> Create(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Length > MAX_LENGTH)
        {
            return Errors.General.ValueLengthIsInvalid(nameof(HealthInfo));
        }

        return new HealthInfo(value);
    }
}