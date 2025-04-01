using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer.Pet.ValueObjects;

public record Size
{
    private Size(float weight, float height)
    {
        Weight = weight;
        Height = height;
    }

    public float Weight { get; }

    public float Height { get; }

    public static Result<Size> Create(float weight, float height)
    {
        if (weight <= 0)
        {
            return Result.Failure<Size>("Weight must be greater than 0");
        }

        if (height <= 0)
        {
            return Result.Failure<Size>("Height must be greater than 0");
        }

        return new Size(weight, height);
    }
}