using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Domain.Shared.Enums;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    Genders Gender,
    string PhoneNumber,
    string Email,
    int Experience);