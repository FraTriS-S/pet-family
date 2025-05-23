using PetFamily.Application.DTOs.Shared;
using PetFamily.Domain.Shared.Enums;
using PetFamily.Domain.Volunteer.Pet.Enums;

namespace PetFamily.Application.Volunteers.Pets.Add;

public record AddPetCommand(
    Guid VolunteerId,
    string Name,
    string? Description,
    Genders Gender,
    Guid SpeciesId,
    Guid BreedId,
    string Color,
    float Weight,
    float Height,
    string? HealthInfo,
    HelpStatuses HelpStatus,
    AddressDto Address,
    DateOnly BirthDate,
    bool IsNeutered,
    bool IsVaccinated,
    string VolunteerPhoneNumber);

public record CreateFileDto(Stream Content, string FileName);