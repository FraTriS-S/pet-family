using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Species;

public class Species : Entity<SpeciesId>
{
    private Species(SpeciesId id) : base(id)
    {
    }

    public Species(SpeciesId id, string name)
        : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; } = null!;

    public IReadOnlyList<Breed.Breed> Breeds { get; private set; } = [];
}