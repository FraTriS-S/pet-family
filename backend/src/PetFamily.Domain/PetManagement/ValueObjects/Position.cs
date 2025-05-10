using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class Position : ValueObject
{
    private const int MIN_VALUE = 0;

    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public Result<Position, Error> Forward()
        => Create(Value + 1);

    public Result<Position, Error> Back()
        => Create(Value - 1);

    public static Result<Position, Error> Create(int value)
    {
        if (value <= MIN_VALUE)
        {
            return Errors.General.ValueIsInvalid(nameof(Position));
        }

        return new Position(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator int(Position position) => position.Value;
}