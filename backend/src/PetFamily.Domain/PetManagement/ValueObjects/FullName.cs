using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record FullName
{
    public const int MAX_LENGTH = 100;

    private FullName(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public string FirstName { get; }

    public string LastName { get; }

    public string MiddleName { get; }

    public static Result<FullName, Error> Create(string firstName, string lastName, string middleName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > MAX_LENGTH)
        {
            return Errors.General.ValueIsInvalid(nameof(FirstName));
        }

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > MAX_LENGTH)
        {
            return Errors.General.ValueIsInvalid(nameof(LastName));
        }

        if (string.IsNullOrWhiteSpace(middleName) || middleName.Length > MAX_LENGTH)
        {
            return Errors.General.ValueIsInvalid(nameof(MiddleName));
        }

        return new FullName(firstName, lastName, middleName);
    }
}