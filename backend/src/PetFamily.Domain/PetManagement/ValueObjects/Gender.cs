using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Enums;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Gender
{
    public Genders Value { get; }

    private Gender(Genders value)
    {
        Value = value;
    }

    public static Result<Gender, Error> Create(Genders value)
    {
        if (value == Genders.Unknown)
        {
            return Errors.General.ValueIsInvalid(nameof(Gender));
        }

        return new Gender(value);
    }
};