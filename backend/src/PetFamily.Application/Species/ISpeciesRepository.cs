using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    Task<Result<bool, Error>> IsSpeciesAndBreedExistsAsync(SpeciesId speciesId, BreedId breedId, CancellationToken cancellationToken);
}