using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoHandler(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    IValidator<UpdateVolunteerMainInfoCommand> validator,
    ILogger<UpdateVolunteerMainInfoHandler> logger)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<UpdateVolunteerMainInfoCommand> _validator = validator;
    private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger = logger;

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
        var gender = Gender.Create(command.Gender).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var email = Email.Create(command.Email).Value;
        var experience = Experience.Create(command.Experience).Value;

        volunteer.UpdateMainInfo(fullName, description, gender, phoneNumber, email, experience);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer with id {Id} was updated", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}