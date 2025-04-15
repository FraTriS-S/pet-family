using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record PaymentDetails
{
    [JsonConstructor]
    private PaymentDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }

    public string Description { get; }

    public static Result<PaymentDetails, Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Errors.General.ValueIsInvalid(nameof(Name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return Errors.General.ValueIsInvalid(nameof(Description));
        }

        return new PaymentDetails(name, description);
    }
}