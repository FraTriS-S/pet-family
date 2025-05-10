using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;
using PetFamily.Domain.Volunteer.Pet.Enums;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

public class Volunteer : Shared.Entity<VolunteerId>
{
    private bool _isDeleted;
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

    public UnitResult<Error> AddPet(Pet pet)
    {
        var positionResult = Position.Create(_pets.Count + 1);

        if (positionResult.IsFailure)
        {
            return positionResult.Error;
        }

        pet.MovePosition(positionResult.Value);

        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePetToFirstPosition(Pet pet)
    {
        var currentPosition = pet.Position;

        var newPositionResult = Position.Create(1);

        if (newPositionResult.IsFailure)
        {
            return newPositionResult.Error;
        }

        var newPosition = newPositionResult.Value;

        if (currentPosition == newPosition || _pets.Count == 1)
        {
            return Result.Success<Error>();
        }

        var moveResult = MovePetsBetweenPositions(newPosition, currentPosition);

        if (moveResult.IsFailure)
        {
            return moveResult.Error;
        }

        pet.MovePosition(newPosition);

        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePetToLastPosition(Pet pet)
    {
        var currentPosition = pet.Position;

        var newPositionResult = Position.Create(_pets.Count);

        if (newPositionResult.IsFailure)
        {
            return newPositionResult.Error;
        }

        var newPosition = newPositionResult.Value;

        if (currentPosition == newPosition || _pets.Count == 1)
        {
            return Result.Success<Error>();
        }

        var moveResult = MovePetsBetweenPositions(newPosition, currentPosition);

        if (moveResult.IsFailure)
        {
            return moveResult.Error;
        }

        pet.MovePosition(newPosition);

        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;

        if (currentPosition == newPosition || _pets.Count == 1)
        {
            return Result.Success<Error>();
        }

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);

        if (adjustedPosition.IsFailure)
        {
            return adjustedPosition.Error;
        }

        newPosition = adjustedPosition.Value;

        var moveResult = MovePetsBetweenPositions(newPosition, currentPosition);

        if (moveResult.IsFailure)
        {
            return moveResult.Error;
        }

        pet.MovePosition(newPosition);

        return Result.Success<Error>();
    }

    public void Delete()
    {
        if (!_isDeleted)
        {
            _isDeleted = true;

            foreach (var pet in _pets)
            {
                pet.Delete();
            }
        }
    }

    public void Restore()
    {
        if (_isDeleted)
        {
            _isDeleted = false;

            foreach (var pet in _pets)
            {
                pet.Restore();
            }
        }
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition <= _pets.Count)
        {
            return newPosition;
        }

        var lastPosition = Position.Create(_pets.Count - 1);

        if (lastPosition.IsFailure)
        {
            return lastPosition.Error;
        }

        return lastPosition;
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition < currentPosition)
        {
            var petsToMove = _pets
                .Where(p => p.Position >= newPosition &&
                            p.Position < currentPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MovePositionForward();

                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }
        else if (newPosition > currentPosition)
        {
            var petsToMove = _pets
                .Where(p => p.Position > currentPosition &&
                            p.Position <= newPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MovePositionBack();

                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }

        return Result.Success<Error>();
    }
}