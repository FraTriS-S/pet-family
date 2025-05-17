using PetFamily.Application.Volunteers.AddPet;

namespace PetFamily.Application.Files.Upload;

public record UploadFilesCommand(IEnumerable<CreateFileDto> Files);