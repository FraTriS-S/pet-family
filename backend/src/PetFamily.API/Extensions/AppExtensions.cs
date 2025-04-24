using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure;

namespace PetFamily.API.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
    }
}