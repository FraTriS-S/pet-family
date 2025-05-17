using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.UpdatePaymentDetails;

public class UpdateVolunteerPaymentDetailsHandler(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    IValidator<UpdateVolunteerPaymentDetailsCommand> validator,
    ILogger<UpdateVolunteerPaymentDetailsHandler> logger)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<UpdateVolunteerPaymentDetailsCommand> _validator = validator;
    private readonly ILogger<UpdateVolunteerPaymentDetailsHandler> _logger = logger;

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

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer with id {Id} was updated", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}