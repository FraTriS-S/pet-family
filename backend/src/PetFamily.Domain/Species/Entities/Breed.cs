using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Species.Entities;

public class Breed : Entity<BreedId>
{
    //ef core constructor
#pragma warning disable CS8618, CS9264
    // ReSharper disable once UnusedMember.Local
    private Breed(BreedId id) : base(id)
#pragma warning restore CS8618, CS9264
    {
    }

    public Breed(BreedId id, BreedName name)
        : base(id)
    {
        Name = name;
    }

    public BreedName Name { get; private set; } = null!;
}