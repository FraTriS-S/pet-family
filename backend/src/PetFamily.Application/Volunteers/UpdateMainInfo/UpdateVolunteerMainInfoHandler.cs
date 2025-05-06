using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerMainInfoCommand> validator,
    ILogger<UpdateVolunteerMainInfoHandler> logger)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository ?? throw new ArgumentNullException(nameof(volunteersRepository));
    private readonly IValidator<UpdateVolunteerMainInfoCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateVolunteerMainInfoCommand command, CancellationToken cancellationToken = default)
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
            
        var fullName = FullName.Create(command.FullName.FirstName, command.FullName.LastName, command.FullName.MiddleName).Value;
        var description = Description.Create(command.Description).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var experience = Experience.Create(command.Experience).Value;

        volunteer.UpdateMainInfo(fullName, description, experience, phoneNumber);
        
        var volunteerGuid = await _volunteersRepository.SaveAsync(volunteer, cancellationToken);
        
        _logger.LogInformation("Volunteer with id {Id} was updated", volunteer.Id.Value);
        
        return volunteerGuid;
    }
}