using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer.Pet.ValueObjects;

public class Weight
{
    private const int MIN_VALUE = 0;

    private Weight(float value)
    {
        Value = value;
    }

    public float Value { get; }

    public static Result<Weight, Error> Create(float value)
    {
        if (value < MIN_VALUE)
        {
            return Errors.General.ValueIsInvalid(nameof(value));
        }

        return new Weight(value);
    }
}