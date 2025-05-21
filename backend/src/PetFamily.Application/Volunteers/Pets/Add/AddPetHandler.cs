using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.Pets.Add;

public class AddPetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> logger,
        IValidator<AddPetCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _volunteersRepository
            .GetByIdAsync(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var speciesId = SpeciesId.Create(command.SpeciesId);
        var breedId = command.BreedId;

        var isSpeciesAndBreedExistsResult = await _speciesRepository.IsSpeciesAndBreedExistsAsync(
            speciesId, BreedId.Create(breedId), cancellationToken);

        if (isSpeciesAndBreedExistsResult.IsFailure)
        {
            return isSpeciesAndBreedExistsResult.Error.ToErrorList();
        }

        if (isSpeciesAndBreedExistsResult.Value == false)
        {
            return Error.NotFound("species.or.breed.not.exist", "Not found species or breed by id").ToErrorList();
        }

        var petId = PetId.NewPetId();
        var petName = PetName.Create(command.Name).Value;
        var description = Description.Create(command.Description).Value;
        var gender = Gender.Create(command.Gender).Value;
        var color = Color.Create(command.Color).Value;
        var weight = Weight.Create(command.Weight).Value;
        var height = Height.Create(command.Height).Value;
        var healthInfo = HealthInfo.Create(command.HealthInfo).Value;
        var helpStatus = HelpStatus.Create(command.HelpStatus).Value;
        var address = Address.Create(
            command.Address.Country,
            command.Address.City,
            command.Address.Street,
            command.Address.House,
            command.Address.Block).Value;
        var birthDate = command.BirthDate;
        var isNeutered = command.IsNeutered;
        var isVaccinated = command.IsVaccinated;
        var volunteerPhoneNumber = PhoneNumber.Create(command.VolunteerPhoneNumber).Value;

        var pet = new Pet(
            petId,
            petName,
            description,
            gender,
            speciesId,
            breedId,
            color,
            weight,
            height,
            healthInfo,
            helpStatus,
            address,
            birthDate,
            isNeutered,
            isVaccinated,
            volunteerPhoneNumber);

        volunteerResult.Value.AddPet(pet);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pet({petId}) added to volunteer({volunteerId})",
            petId.Value, volunteerId.Value);

        return pet.Id.Value;
    }
}