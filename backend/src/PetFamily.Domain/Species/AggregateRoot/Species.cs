using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Species.AggregateRoot;

public class Species : Entity<SpeciesId>
{
    //ef core constructor
#pragma warning disable CS8618, CS9264
    // ReSharper disable once UnusedMember.Local
    private Species(SpeciesId id) : base(id)
#pragma warning restore CS8618, CS9264
    {
    }

    public Species(SpeciesId id, SpeciesName name)
        : base(id)
    {
        Name = name;
    }

    public SpeciesName Name { get; private set; } = null!;

    public IReadOnlyList<Entities.Breed> Breeds { get; private set; } = [];
}