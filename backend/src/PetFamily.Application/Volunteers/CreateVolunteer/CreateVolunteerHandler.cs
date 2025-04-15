using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteer;
using PetFamily.Domain.Volunteer.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository ?? throw new ArgumentNullException(nameof(volunteersRepository));

    public async Task<Result<Guid, Error>> HandleAsync(
        CreateVolunteerRequest request, CancellationToken cancellationToken = default)
    {
        // валидация

        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        var existedVolunteer = await _volunteersRepository.GetByEmailAsync(emailResult.Value, cancellationToken);

        if (existedVolunteer.IsSuccess)
        {
            return Errors.Volunteer.AlreadyExist();
        }

        var volunteerId = VolunteerId.NewVolunteerId();

        var fullNameResult = FullName.Create(request.FirstName, request.LastName, request.MiddleName);

        if (fullNameResult.IsFailure)
        {
            return fullNameResult.Error;
        }

        var descriptionResult = Description.Create(request.Description);

        if (descriptionResult.IsFailure)
        {
            return descriptionResult.Error;
        }

        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);

        if (phoneNumberResult.IsFailure)
        {
            return phoneNumberResult.Error;
        }

        var experienceResult = Experience.Create(0);

        if (experienceResult.IsFailure)
        {
            return experienceResult.Error;
        }

        var volunteer = new Volunteer(
            volunteerId,
            fullNameResult.Value,
            descriptionResult.Value,
            request.Gender,
            phoneNumberResult.Value,
            emailResult.Value,
            experienceResult.Value
        );

        await _volunteersRepository.AddAsync(volunteer, cancellationToken);

        return (Guid)volunteer.Id;
    }
}