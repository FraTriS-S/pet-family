using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers.FileProvider;
using PetFamily.Application.Species;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.Pets.Add;

public class AddPetHandler(
    IFileProvider fileProvider,
    IVolunteersRepository volunteersRepository,
    ISpeciesRepository speciesRepository,
    IUnitOfWork unitOfWork,
    ILogger<AddPetHandler> logger,
    IValidator<AddPetCommand> validator)
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider = fileProvider;
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository = speciesRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<AddPetHandler> _logger = logger;
    private readonly IValidator<AddPetCommand> _validator = validator;

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var volunteerResult = await _volunteersRepository
                .GetByIdAsync(VolunteerId.Create(command.VolunteerId), cancellationToken);

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

            List<FileData> filesData = [];

            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                {
                    return filePath.Error.ToErrorList();
                }

                var fileContent = new FileData(file.Content, filePath.Value, BUCKET_NAME);

                filesData.Add(fileContent);
            }

            var petPhotos = filesData
                .Select(f => f.FilePath)
                .Select(f => new Photo(f))
                .ToList();

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
                volunteerPhoneNumber,
                petPhotos);

            volunteerResult.Value.AddPet(pet);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var uploadResult = await _fileProvider.UploadFilesAsync(filesData, cancellationToken);

            if (uploadResult.IsFailure)
            {
                return uploadResult.Error.ToErrorList();
            }

            transaction.Commit();

            return pet.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Can not add pet to volunteer - {id} in transaction", command.VolunteerId);

            transaction.Rollback();

            return Error.Failure(
                    $"Can not add pet to volunteer - {command.VolunteerId}", "module.issue.failure")
                .ToErrorList();
        }
    }
}