using PetFamily.Application.Volunteers.Pets.Add;

namespace PetFamily.Application.Volunteers.Pets.AddPhotos;

public record UploadPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreateFileDto> Photos);