using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer.Pet.ValueObjects;

public record HealthInfo
{
    private HealthInfo(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<HealthInfo> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<HealthInfo>("HealthInfo is required");
        }

        return new HealthInfo(value);
    }
}