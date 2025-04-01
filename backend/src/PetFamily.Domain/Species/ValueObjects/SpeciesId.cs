namespace PetFamily.Domain.Species.ValueObjects;

public record SpeciesId
{
    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static SpeciesId NewPetId() => new(Guid.NewGuid());
    
    public static SpeciesId Empty() => new(Guid.Empty);
}