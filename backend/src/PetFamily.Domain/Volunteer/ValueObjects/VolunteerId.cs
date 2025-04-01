namespace PetFamily.Domain.Volunteer.ValueObjects;

public record VolunteerId
{
    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());

    public static VolunteerId Empty() => new(Guid.Empty);
}