using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerSocialNetworksCommand> validator,
    ILogger<UpdateVolunteerSocialNetworksHandler> logger)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly IValidator<UpdateVolunteerSocialNetworksCommand> _validator = validator;
    private readonly ILogger<UpdateVolunteerSocialNetworksHandler> _logger = logger;

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateVolunteerSocialNetworksCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _volunteersRepository.GetByIdAsync(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var volunteer = volunteerResult.Value;

        var socialNetworks = command.SocialNetworks.Select(x => SocialNetwork.Create(x.Name, x.Url).Value);

        volunteer.UpdateSocialNetworks(socialNetworks);

        var volunteerGuid = await _volunteersRepository.SaveAsync(volunteer, cancellationToken);

        _logger.LogInformation("Volunteer with id {Id} was updated", volunteer.Id.Value);

        return volunteerGuid;
    }
}