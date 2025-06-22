using Muuki.DTOs;
using Muuki.Models;

namespace Muuki.Services.Interfaces
{
    public interface IAnimalService
    {
        Task<List<Animal>> GetAllAnimals(string userId);
        Task<Animal?> GetAnimalById(string userId, string animalId);
        Task<List<Animal>> CreateAnimals(string userId, string spaceId, AnimalCreateDto dto);
        Task<Animal> UpdateAnimal(string userId, string animalId, AnimalUpdateDto dto);
        Task<bool> DeleteAnimal(string userId, string animalId);
        Task<Animal> UpdateBreedName(string userId, string animalId, string oldBreedName, string newBreedName);
        Task<Animal> DeleteBreed(string userId, string animalId, string breedName);
    }
}