namespace PetFamily.Application.Volunteers.Pets.RemovePhotos;

public record RemovePetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> PhotoNames);