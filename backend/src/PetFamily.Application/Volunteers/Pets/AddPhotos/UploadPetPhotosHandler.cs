using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers.FileProvider;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.Pets.AddPhotos;

public class UploadPetPhotosHandler(
    IFileProvider fileProvider,
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    IValidator<UploadPetPhotosCommand> validator,
    ILogger<UploadPetPhotosHandler> logger)
{
    private readonly IFileProvider _fileProvider = fileProvider;
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<UploadPetPhotosCommand> _validator = validator;
    private readonly ILogger<UploadPetPhotosHandler> _logger = logger;

    private const string BUCKET_NAME = "photos";

    public async Task<Result<List<string>, ErrorList>> HandleAsync(
        UploadPetPhotosCommand command, CancellationToken cancellationToken = default)
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

        List<FileData> photosData = [];

        foreach (var photo in command.Photos)
        {
            var extension = Path.GetExtension(photo.FileName);

            var photoPath = FilePath.Create(Guid.NewGuid(), extension);

            if (photoPath.IsFailure)
            {
                return photoPath.Error.ToErrorList();
            }

            var photoContent = new FileData(photo.Content, photoPath.Value, BUCKET_NAME);

            photosData.Add(photoContent);
        }

        var uploadResult = await _fileProvider.UploadFilesAsync(photosData, cancellationToken);

        if (uploadResult.IsFailure)
        {
            return uploadResult.Error.ToErrorList();
        }

        var petPhotos = photosData
            .Select(f => f.FilePath)
            .Select(f => new Photo(f))
            .ToList();

        var addPhotosResult = petResult.Value.AddPhotos(petPhotos);

        if (uploadResult.IsFailure)
        {
            return addPhotosResult.Error.ToErrorList();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Photos for pet upload completed");

        var photosPaths = photosData.Select(x => x.FilePath.Path);

        return photosPaths.ToList();
    }
}