using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.PresignedGet;
using PetFamily.Application.Files.Upload;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Pets.Add;
using PetFamily.Application.Volunteers.Pets.AddPhotos;
using PetFamily.Application.Volunteers.Pets.MovePosition;
using PetFamily.Application.Volunteers.Pets.RemovePhotos;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdatePaymentDetails;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<HardDeleteVolunteerHandler>();
        services.AddScoped<UpdateVolunteerMainInfoHandler>();
        services.AddScoped<UpdateVolunteerSocialNetworksHandler>();
        services.AddScoped<UpdateVolunteerPaymentDetailsHandler>();

        services.AddScoped<AddPetHandler>();
        services.AddScoped<UploadPetPhotosHandler>();
        services.AddScoped<RemovePetPhotosHandler>();
        services.AddScoped<MovePetPositionHandler>();

        services.AddScoped<PresignedGetFileHandler>();
        services.AddScoped<UploadFileHandler>();
        services.AddScoped<RemoveFileHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}