namespace PetFamily.Application.Volunteers.Pets.MovePosition;

public record MovePetPositionCommand(Guid VolunteerId, Guid PetId, int Position);