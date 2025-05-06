using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateVolunteerMainInfoRequest(
    Guid VolunteerId,
    UpdateVolunteerMainInfoDto Dto)
{
    public UpdateVolunteerMainInfoCommand ToCommand() =>
        new(VolunteerId, Dto.FullName, Dto.Description, Dto.Experience, Dto.PhoneNumber);
}

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber);