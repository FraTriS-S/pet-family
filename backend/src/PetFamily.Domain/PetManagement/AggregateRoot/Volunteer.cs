using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Ids;
using PetFamily.Domain.Volunteer.Pet.Enums;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

public class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<PaymentDetails> _paymentDetails = [];

    //ef core navigation
    public IReadOnlyList<Pet> Pets => _pets;

    //For EfCore
#pragma warning disable CS8618, CS9264
    // ReSharper disable once UnusedMember.Local
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
        IEnumerable<SocialNetwork> socialNetworks,
        IEnumerable<PaymentDetails> paymentDetails
    ) : base(id)
    {
        FullName = fullName;
        Description = description;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Email = email;
        Experience = experience;
        _socialNetworks = socialNetworks.ToList();
        _paymentDetails = paymentDetails.ToList();
    }

    public FullName FullName { get; private set; }

    public Description Description { get; private set; }

    public Gender Gender { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public Email Email { get; private set; }

    public Experience Experience { get; private set; }

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public IReadOnlyList<PaymentDetails> PaymentDetails => _paymentDetails;

    public int GetNumberOfPetsWithHome() => _pets.Count(x => x.HelpStatus.Value == HelpStatuses.FoundHome);

    public int GetNumberOfPetsLookingHome() => _pets.Count(x => x.HelpStatus.Value == HelpStatuses.LookingHome);

    public int GetNumberOfPetsFoundHome() => _pets.Count(x => x.HelpStatus.Value == HelpStatuses.FoundHome);

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        Gender gender,
        PhoneNumber phoneNumber,
        Email email,
        Experience experience)
    {
        FullName = fullName;
        Description = description;
        Gender = gender;
        Experience = experience;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        _socialNetworks.Clear();
        _socialNetworks.AddRange(socialNetworks.ToList());
    }

    public void UpdatePaymentDetails(IEnumerable<PaymentDetails> paymentDetails)
    {
        _paymentDetails.Clear();
        _paymentDetails.AddRange(paymentDetails.ToList());
    }
}