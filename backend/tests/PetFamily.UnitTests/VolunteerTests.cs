using FluentAssertions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Enums;
using PetFamily.Domain.Shared.Ids;
using PetFamily.Domain.Volunteer.Pet.Enums;

namespace PetFamily.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Pet_With_Empty_Pets_Return_Success_Result()
    {
        // arrange
        const int petsCount = 0;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var petId = PetId.NewPetId();
        var name = PetName.Create("Test").Value;
        var description = Description.Create("Test").Value;
        var gender = Gender.Create(Genders.Boy).Value;
        var speciesId = SpeciesId.NewSpeciesId();
        var breedId = Guid.NewGuid();
        var color = Color.Create("Test").Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var healthInfo = HealthInfo.Create("Test").Value;
        var helpStatus = HelpStatus.Create(HelpStatuses.FoundHome).Value;
        var address = Address.Create("Test", "Test", "Test", "Test", 1).Value;
        var birthDate = DateOnly.MinValue;
        var isNeutered = true;
        var isVaccinated = true;
        var phoneNumber = PhoneNumber.Create("88005553535").Value;

        var pet = new Pet(petId, name, description, gender, speciesId, breedId, color, weight, height, healthInfo,
            helpStatus, address, birthDate, isNeutered, isVaccinated, phoneNumber, []);

        // act
        var result = volunteer.AddPet(pet);

        // assert
        var addedPet = volunteer.Pets.Single(p => p.Id == petId);

        Assert.True(result.IsSuccess);
        Assert.Equal(addedPet.Id, pet.Id);
        Assert.Equal(addedPet.Position, Position.Create(1));
    }

    [Fact]
    public void Add_Pet_With_Other_Pets_Time_Return_Success_Result()
    {
        // arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);

        var name = PetName.Create("Test").Value;
        var description = Description.Create("Test").Value;
        var gender = Gender.Create(Genders.Boy).Value;
        var speciesId = SpeciesId.NewSpeciesId();
        var breedId = Guid.NewGuid();
        var color = Color.Create("Test").Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var healthInfo = HealthInfo.Create("Test").Value;
        var helpStatus = HelpStatus.Create(HelpStatuses.FoundHome).Value;
        var address = Address.Create("Test", "Test", "Test", "Test", 1).Value;
        var birthDate = DateOnly.MinValue;
        var isNeutered = true;
        var isVaccinated = true;
        var phoneNumber = PhoneNumber.Create("88005553535").Value;

        var petToAdd = new Pet(PetId.NewPetId(), name, description, gender, speciesId, breedId, color, weight, height, healthInfo,
            helpStatus, address, birthDate, isNeutered, isVaccinated, phoneNumber, []);

        // act
        var result = volunteer.AddPet(petToAdd);

        // assert
        var addedPet = volunteer.Pets.Single(p => p.Id == petToAdd.Id);

        Assert.True(result.IsSuccess);
        Assert.Equal(addedPet.Id, petToAdd.Id);
        Assert.Equal(addedPet.Position, Position.Create(petsCount + 1));
    }

    [Fact]
    public void Move_Pet_Should_Not_Move_When_Pet_Already_At_New_Position()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(secondPet, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
        fourthPet.Position.Value.Should().Be(4);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void Move_Pet_Should_Move_Other_Pets_Forward_When_New_Position_Is_Lower()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(fourthPet, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(2);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void Move_Pet_Should_Move_Other_Pets_Back_When_New_Position_Is_Greater()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var forthPosition = Position.Create(4).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(secondPet, forthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(4);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void Move_Pet_Should_Move_Other_Pets_Forward_When_New_Position_Is_First()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var firstPosition = Position.Create(1).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(fifthPet, firstPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(5);
        fifthPet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void Move_Pet_Should_Move_Other_Pets_Back_When_New_Position_Is_Last()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fifthPosition = Position.Create(5).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(firstPet, fifthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(5);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(4);
    }

    [Fact]
    public void Move_Pet_To_First_Position_Should_Move_Other_Pets_Forward_When_New_Position_Is_First()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePetToFirstPosition(fifthPet);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(5);
        fifthPet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void Move_Pet_To_Last_Position_Should_Move_Other_Pets_Back_When_New_Position_Is_Last()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePetToLastPosition(firstPet);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(5);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(4);
    }

    private Volunteer CreateVolunteerWithPets(int petsCount)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("Test", "Test", "Test").Value;
        var description = Description.Create("Test").Value;
        var gender = Gender.Create(Genders.Boy).Value;
        var phoneNumber = PhoneNumber.Create("88005553535").Value;
        var email = Email.Create("test@gmail.com").Value;
        var experience = Experience.Create(1).Value;

        var volunteer = new Volunteer(volunteerId, fullName, description, gender, phoneNumber, email, experience, [], []);

        var name = PetName.Create("Test").Value;
        var speciesId = SpeciesId.NewSpeciesId();
        var breedId = Guid.NewGuid();
        var color = Color.Create("Test").Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var healthInfo = HealthInfo.Create("Test").Value;
        var helpStatus = HelpStatus.Create(HelpStatuses.FoundHome).Value;
        var address = Address.Create("Test", "Test", "Test", "Test", 1).Value;
        var birthDate = DateOnly.MinValue;
        var isNeutered = true;
        var isVaccinated = true;

        for (var i = 0; i < petsCount; i++)
        {
            var pet = new Pet(PetId.NewPetId(), name, description, gender, speciesId, breedId, color, weight, height, healthInfo,
                helpStatus, address, birthDate, isNeutered, isVaccinated, phoneNumber, []);
            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}