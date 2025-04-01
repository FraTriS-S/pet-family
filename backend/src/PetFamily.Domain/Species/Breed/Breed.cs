using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Species.Breed.ValueObjects;

namespace PetFamily.Domain.Species.Breed;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }

    public Breed(BreedId id, Name name)
        : base(id)
    {
        Name = name;
    }

    public Name Name { get; private set; } = null!;
}