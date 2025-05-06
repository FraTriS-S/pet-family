using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateVolunteerSocialNetworksRequest(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateVolunteerSocialNetworksCommand ToCommand() =>
        new(VolunteerId, SocialNetworks);
}