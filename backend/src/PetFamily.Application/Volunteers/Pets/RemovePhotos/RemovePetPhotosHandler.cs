using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers.FileProvider;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.Pets.RemovePhotos;

public class RemovePetPhotosHandler(
    IFileProvider fileProvider,
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    IValidator<RemovePetPhotosCommand> validator,
    ILogger<RemovePetPhotosHandler> logger)
{
    private readonly IFileProvider _fileProvider = fileProvider;
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<RemovePetPhotosCommand> _validator = validator;
    private readonly ILogger<RemovePetPhotosHandler> _logger = logger;

    public async Task<Result<List<string>, ErrorList>> HandleAsync(
        RemovePetPhotosCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _volunteersRepository.GetByIdAsync(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);

        if (petResult.IsFailure)
        {
            return petResult.Error.ToErrorList();
        }

        var removeResult = await _fileProvider.RemoveFilesAsync(command.PhotoNames, cancellationToken);

        if (removeResult.IsFailure)
        {
            return removeResult.Error.ToErrorList();
        }

        List<FilePath> photoPaths = [];

        foreach (var photoName in command.PhotoNames)
        {
            var photoPath = FilePath.Create(photoName);

            if (photoPath.IsFailure)
            {
                return photoPath.Error.ToErrorList();
            }

            photoPaths.Add(photoPath.Value);
        }

        var petPhotos = photoPaths
            .Select(f => new Photo(f))
            .ToList();

        var removePhotosResult = petResult.Value.RemovePhotos(petPhotos);

        if (removePhotosResult.IsFailure)
        {
            return removePhotosResult.Error.ToErrorList();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Photos for pet were removed");

        var photosPaths = photoPaths.Select(x => x.Path);

        return photosPaths.ToList();
    }
}