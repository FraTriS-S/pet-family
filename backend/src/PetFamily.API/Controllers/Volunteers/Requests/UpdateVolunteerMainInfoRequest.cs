using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared.Enums;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateVolunteerMainInfoRequest(
    Guid VolunteerId,
    UpdateVolunteerMainInfoDto Dto)
{
    public UpdateVolunteerMainInfoCommand ToCommand() =>
        new(VolunteerId,
            Dto.FullName,
            Dto.Description,
            Dto.Gender,
            Dto.PhoneNumber,
            Dto.Email,
            Dto.Experience);
}

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullName,
    string Description,
    Genders Gender,
    string PhoneNumber,
    string Email,
    int Experience);