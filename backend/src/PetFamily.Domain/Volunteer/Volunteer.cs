using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteer.Pet.ValueObjects;
using PetFamily.Domain.Volunteer.ValueObjects;

namespace PetFamily.Domain.Volunteer;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<Pet.Pet> _pets = [];

    //For EfCore
#pragma warning disable CS8618, CS9264
    private Volunteer(VolunteerId id) : base(id)
#pragma warning restore CS8618, CS9264
    {
    }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description description,
        Gender gender,
        PhoneNumber phoneNumber,
        Email email,
        Experience experience,
        PaymentDetails paymentDetails)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Email = email;
        Experience = experience;
        PaymentDetails = paymentDetails;
    }

    public FullName FullName { get; private set; }

    public Description Description { get; private set; }

    public Gender Gender { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public Email Email { get; private set; }

    public Experience Experience { get; private set; }

    public PaymentDetails PaymentDetails { get; private set; }

    public int GetNumberOfPetsWithHome() => _pets.Count(x => x.HelpStatus.Value == HelpStatus.Status.FoundHome);

    public int GetNumberOfPetsLookingHome() => _pets.Count(x => x.HelpStatus.Value == HelpStatus.Status.LookingHome);

    public int GetNumberOfPetsFoundHome() => _pets.Count(x => x.HelpStatus.Value == HelpStatus.Status.FoundHome);

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public IReadOnlyList<Pet.Pet> Pets => _pets;
}