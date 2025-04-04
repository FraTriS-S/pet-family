using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Enums;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteer.Pet.Enums;
using PetFamily.Domain.Volunteer.ValueObjects;

namespace PetFamily.Domain.Volunteer;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet.Pet> _pets = [];
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<PaymentDetails> _paymentDetails = [];

    //ef core navigation
    public IReadOnlyList<Pet.Pet> Pets => _pets;

    //For EfCore
#pragma warning disable CS8618, CS9264
    private Volunteer(VolunteerId id) : base(id)
#pragma warning restore CS8618, CS9264
    {
    }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        string? description,
        Gender gender,
        PhoneNumber phoneNumber,
        Email email,
        int experience)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Email = email;
        Experience = experience;
    }

    public FullName FullName { get; private set; }

    public string? Description { get; private set; }

    public Gender Gender { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public Email Email { get; private set; }

    public int Experience { get; private set; }

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public IReadOnlyList<PaymentDetails> PaymentDetails => _paymentDetails;

    public int GetNumberOfPetsWithHome() => _pets.Count(x => x.HelpStatus == HelpStatus.FoundHome);

    public int GetNumberOfPetsLookingHome() => _pets.Count(x => x.HelpStatus == HelpStatus.LookingHome);

    public int GetNumberOfPetsFoundHome() => _pets.Count(x => x.HelpStatus == HelpStatus.FoundHome);
}