using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<DeleteVolunteerCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<DeleteVolunteerHandler> logger)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly IValidator<DeleteVolunteerCommand> _validator = validator;
    private readonly ILogger<DeleteVolunteerHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteVolunteerCommand command, CancellationToken cancellationToken = default)
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

        volunteer.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer was soft deleted with id: {Id}", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}