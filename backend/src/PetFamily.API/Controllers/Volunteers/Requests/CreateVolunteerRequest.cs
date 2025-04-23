using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Shared.Enums;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string? Description,
    Genders Gender,
    string PhoneNumber,
    string Email,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<PaymentDetailsDto> PaymentDetails
)
{
    public CreateVolunteerCommand ToCommand() =>
        new(
            FullName,
            Description,
            Gender,
            PhoneNumber,
            Email,
            SocialNetworks,
            PaymentDetails
        );
}