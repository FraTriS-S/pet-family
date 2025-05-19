using PetFamily.Application.Volunteers.Pets.Add;

namespace PetFamily.Application.Files.Upload;

public record UploadFilesCommand(IEnumerable<CreateFileDto> Files);