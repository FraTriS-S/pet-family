using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer.ValueObjects;

public record FullName
{
    private FullName(string name, string lastName, string middleName)
    {
        Name = name;
        LastName = lastName;
        MiddleName = middleName;
    }

    public string Name { get; }

    public string LastName { get; }

    public string MiddleName { get; }

    public static Result<FullName> Create(string name, string lastName, string middleName)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<FullName>("Name is required");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<FullName>("LastName is required");
        }

        if (string.IsNullOrWhiteSpace(middleName))
        {
            return Result.Failure<FullName>("MiddleName is required");
        }

        return new FullName(name, lastName, middleName);
    }
}