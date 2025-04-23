using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Shared.Enums;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string MiddleName,
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
            FirstName,
            LastName,
            MiddleName,
            Description,
            Gender,
            PhoneNumber,
            Email,
            SocialNetworks,
            PaymentDetails
        );
}