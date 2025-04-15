using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Shared.Enums;

namespace PetFamily.API.Requests.Volunteers;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string MiddleName,
    string? Description,
    Gender Gender,
    string PhoneNumber,
    string Email,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<PaymentDetailsDto> PaymentDetails
)
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(
            FirstName,
            LastName,
            MiddleName,
            Description,
            Gender,
            PhoneNumber,
            Email,
            SocialNetworks,
            PaymentDetails);
    }
}