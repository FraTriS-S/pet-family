using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.Pets.MovePosition;

public class MovePetPositionHandler(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    IValidator<MovePetPositionCommand> validator,
    ILogger<MovePetPositionHandler> logger)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<MovePetPositionCommand> _validator = validator;
    private readonly ILogger<MovePetPositionHandler> _logger = logger;

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        MovePetPositionCommand command, CancellationToken cancellationToken = default)
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

        var petId = PetId.Create(command.PetId);

        var petResult = volunteer.GetPetById(petId);

        if (petResult.IsFailure)
        {
            return petResult.Error.ToErrorList();
        }

        var position = Position.Create(command.Position).Value;

        var movePetPositionResult = volunteer.MovePet(petResult.Value, position);

        if (movePetPositionResult.IsFailure)
        {
            return movePetPositionResult.Error.ToErrorList();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Photos for pet were removed");

        return petId.Value;
    }
}