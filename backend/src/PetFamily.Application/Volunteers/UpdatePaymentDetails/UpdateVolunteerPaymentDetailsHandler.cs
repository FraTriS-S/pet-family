using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.UpdatePaymentDetails;

public class UpdateVolunteerPaymentDetailsHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerPaymentDetailsCommand> validator,
    ILogger<UpdateVolunteerPaymentDetailsHandler> logger)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository ?? throw new ArgumentNullException(nameof(volunteersRepository));
    private readonly IValidator<UpdateVolunteerPaymentDetailsCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    private readonly ILogger<UpdateVolunteerPaymentDetailsHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateVolunteerPaymentDetailsCommand command, CancellationToken cancellationToken = default)
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

        var paymentDetails = command.PaymentDetails.Select(x => PaymentDetails.Create(x.Name, x.Description).Value);

        volunteer.UpdatePaymentDetails(paymentDetails);

        var volunteerGuid = await _volunteersRepository.SaveAsync(volunteer, cancellationToken);

        _logger.LogInformation("Volunteer with id {Id} was updated", volunteer.Id.Value);

        return volunteerGuid;
    }
}