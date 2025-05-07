using PetFamily.Application.DTOs.Volunteer;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks);