using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Domain.Shared.Enums;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string? Description,
    Genders Gender,
    string PhoneNumber,
    string Email,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<PaymentDetailsDto> PaymentDetails
);