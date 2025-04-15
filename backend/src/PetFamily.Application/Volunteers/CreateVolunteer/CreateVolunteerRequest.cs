using PetFamily.Domain.Shared.Enums;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string MiddleName,
    string? Description,
    Gender Gender,
    string PhoneNumber,
    string Email
);