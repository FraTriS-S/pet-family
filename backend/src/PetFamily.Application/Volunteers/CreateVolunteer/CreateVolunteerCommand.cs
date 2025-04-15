using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Domain.Shared.Enums;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerCommand(
    string FirstName,
    string LastName,
    string MiddleName,
    string? Description,
    Gender Gender,
    string PhoneNumber,
    string Email,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<PaymentDetailsDto> PaymentDetails
);