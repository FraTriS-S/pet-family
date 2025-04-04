using PetFamily.Domain.Shared;

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
            return "Name is required";
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return "LastName is required";
        }

        if (string.IsNullOrWhiteSpace(middleName))
        {
            return "MiddleName is required";
        }

        return new FullName(name, lastName, middleName);
    }
}