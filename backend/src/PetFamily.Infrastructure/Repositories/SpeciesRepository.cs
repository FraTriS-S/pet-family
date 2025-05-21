using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SpeciesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<bool, Error>> IsSpeciesAndBreedExistsAsync(
        SpeciesId speciesId, BreedId breedId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Species
            .AnyAsync(s => s.Id == speciesId && s.Breeds
                .Any(b => b.Id == breedId), cancellationToken: cancellationToken);

        return result;
    }
}