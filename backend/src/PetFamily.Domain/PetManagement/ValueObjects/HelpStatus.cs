using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer.Pet.Enums;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record HelpStatus
{
    public HelpStatuses Value { get; }

    private HelpStatus(HelpStatuses value)
    {
        Value = value;
    }

    public static Result<HelpStatus, Error> Create(HelpStatuses value)
    {
        if (value == HelpStatuses.Unknown)
        {
            return Errors.General.ValueIsInvalid(nameof(HelpStatus));
        }

        return new HelpStatus(value);
    }
}