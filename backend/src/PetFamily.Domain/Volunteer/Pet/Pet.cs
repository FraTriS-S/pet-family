using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Species.Breed.ValueObjects;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Volunteer.Pet.ValueObjects;

namespace PetFamily.Domain.Volunteer.Pet;

public class Pet : Shared.Entity<PetId>
{
    //For EfCore
#pragma warning disable CS8618, CS9264
    private Pet(PetId id) : base(id)
#pragma warning restore CS8618, CS9264
    {
    }

    private Pet(
        PetId id,
        Name name,
        Description description,
        Gender gender,
        SpeciesId speciesId,
        BreedId breedId,
        Color color,
        Size petSize,
        HealthInfo healthInfo,
        HelpStatus helpStatus,
        Address address,
        DateOnly birthDate,
        bool isNeutered,
        bool isVaccinated,
        PaymentDetails paymentDetails,
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
        PetSize = petSize;
        HealthInfo = healthInfo;
        HelpStatus = helpStatus;
        Address = address;
        BirthDate = birthDate;
        IsNeutered = isNeutered;
        IsVaccinated = isVaccinated;
        PaymentDetails = paymentDetails;
        CreatedDate = createdDate;
        VolunteerPhoneNumber = volunteerPhoneNumber;
    }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public Gender Gender { get; private set; }

    public SpeciesId SpeciesId { get; private set; }

    public BreedId BreedId { get; private set; }

    public Color Color { get; private set; }

    public Size PetSize { get; private set; }

    public HealthInfo HealthInfo { get; private set; }

    public HelpStatus HelpStatus { get; private set; }

    public Address Address { get; private set; }

    public DateOnly BirthDate { get; private set; }

    public bool IsNeutered { get; private set; }

    public bool IsVaccinated { get; private set; }

    public PaymentDetails PaymentDetails { get; private set; }

    public DateOnly CreatedDate { get; private set; }

    public PhoneNumber VolunteerPhoneNumber { get; private set; }
}