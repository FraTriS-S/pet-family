using System.Text.RegularExpressions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record PhoneNumber
{
    private const string PATTERN = @"^(\+7|7|8)?[\s-]?\(?\d{3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$";

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, PATTERN))
        {
            return "Invalid phone number";
        }

        return new PhoneNumber(value);
    }
}