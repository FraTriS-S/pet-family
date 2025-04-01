using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer.Pet.ValueObjects;

public record HelpStatus
{
    private HelpStatus(Status value)
    {
        Value = value;
    }

    public Status Value { get; }

    public static Result<HelpStatus> Create(Status value)
    {
        return new HelpStatus(value);
    }

    public enum Status
    {
        Unknown = 0,
        NeedsHelp = 1,
        LookingHome = 2,
        FoundHome = 3,
    }
}