using PetFamily.Application.DTOs.Volunteer;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber);