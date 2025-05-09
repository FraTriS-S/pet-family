using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : Shared.Entity<PetId>
{
    private bool _isDeleted;
    private readonly List<PaymentDetails> _paymentDetails = [];

    //ef core navigation
    public PetManagement.AggregateRoot.Volunteer Volunteer { get; private set; } = null!;

    //ef core constructor
#pragma warning disable CS8618, CS9264
    // ReSharper disable once UnusedMember.Local
    private Pet(PetId id) : base(id)
#pragma warning restore CS8618, CS9264
    {
    }

    public Pet(
        PetId id,
        PetName name,
        Description description,
        Gender gender,
        SpeciesId speciesId,
        Guid breedId,
        Color color,
        Weight weight,
        Height height,
        HealthInfo healthInfo,
        HelpStatus helpStatus,
        Address address,
        DateOnly birthDate,
        bool isNeutered,
        bool isVaccinated,
        DateOnly createdDate,
        PhoneNumber volunteerPhoneNumber
    ) : base(id)
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

    public PetName Name { get; private set; }

    public Description Description { get; private set; }

    public Gender Gender { get; private set; }

    public SpeciesId SpeciesId { get; private set; }

    public Guid BreedId { get; private set; }

    public Color Color { get; private set; }

    public Weight Weight { get; private set; }

    public Height Height { get; private set; }

    public HealthInfo HealthInfo { get; private set; }

    public HelpStatus HelpStatus { get; private set; }

    public Address Address { get; private set; }

    public DateOnly BirthDate { get; private set; }

    public bool IsNeutered { get; private set; }

    public bool IsVaccinated { get; private set; }

    public DateOnly CreatedDate { get; private set; }

    public PhoneNumber VolunteerPhoneNumber { get; private set; }

    public IReadOnlyList<PaymentDetails> PaymentDetails => _paymentDetails;

    public void Delete()
    {
        if (!_isDeleted)
        {
            _isDeleted = true;
        }
    }

    public void Restore()
    {
        if (_isDeleted)
        {
            _isDeleted = false;
        }
    }
}