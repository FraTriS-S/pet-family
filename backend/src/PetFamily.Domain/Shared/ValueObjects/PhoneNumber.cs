using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber> Create(string value)
    {
        var pattern = @"^(\+7|7|8)?[\s-]?\(?\d{3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$";

        if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, pattern))
        {
            return Result.Failure<PhoneNumber>("Invalid phone number");
        }

        return new PhoneNumber(value);
    }
}