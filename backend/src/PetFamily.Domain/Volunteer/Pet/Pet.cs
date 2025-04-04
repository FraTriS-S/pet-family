using PetFamily.Domain.Shared.Enums;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Species.Breed.ValueObjects;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Volunteer.Pet.Enums;
using PetFamily.Domain.Volunteer.Pet.ValueObjects;

namespace PetFamily.Domain.Volunteer.Pet;

public class Pet : Shared.Entity<PetId>
{
    private readonly List<PaymentDetails> _paymentDetails = [];

    //ef core navigation
    public Volunteer Volunteer { get; private set; } = null!;

    //ef core constructor
#pragma warning disable CS8618, CS9264
    private Pet(PetId id) : base(id)
#pragma warning restore CS8618, CS9264
    {
    }

    public Pet(
        PetId id,
        string name,
        string? description,
        Gender gender,
        SpeciesId speciesId,
        BreedId breedId,
        string color,
        float weight,
        float height,
        string? healthInfo,
        HelpStatus helpStatus,
        Address address,
        DateOnly birthDate,
        bool isNeutered,
        bool isVaccinated,
        DateOnly createdDate,
        PhoneNumber volunteerPhoneNumber
    )
        : base(id)
    {
        Name = name;
        Description = description;
        Gender = gender;
        SpeciesId = speciesId;
        BreedId = breedId;
        Color = color;
        Weight = weight;
        Height = height;
        HealthInfo = healthInfo;
        HelpStatus = helpStatus;
        Address = address;
        BirthDate = birthDate;
        IsNeutered = isNeutered;
        IsVaccinated = isVaccinated;
        CreatedDate = createdDate;
        VolunteerPhoneNumber = volunteerPhoneNumber;
    }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public Gender Gender { get; private set; }

    public SpeciesId SpeciesId { get; private set; }

    public BreedId BreedId { get; private set; }

    public string Color { get; private set; }

    public float Weight { get; private set; }

    public float Height { get; private set; }


    public string? HealthInfo { get; private set; }

    public HelpStatus HelpStatus { get; private set; }

    public Address Address { get; private set; }

    public DateOnly BirthDate { get; private set; }

    public bool IsNeutered { get; private set; }

    public bool IsVaccinated { get; private set; }

    public DateOnly CreatedDate { get; private set; }

    public PhoneNumber VolunteerPhoneNumber { get; private set; }

    public IReadOnlyList<PaymentDetails> PaymentDetails => _paymentDetails;
}