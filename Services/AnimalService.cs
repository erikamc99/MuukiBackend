using Muuki.DTOs;
using Muuki.Models;
using Muuki.Data;
using Muuki.Services.Interfaces;
using Muuki.Exceptions;
using MongoDB.Driver;

namespace Muuki.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly MongoContext _context;

        public AnimalService(MongoContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> GetAllAnimals(string userId)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            return spaces.SelectMany(s => s.Animals).ToList();
        }

        public async Task<Animal?> GetAnimalById(string userId, string animalId)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            return spaces.SelectMany(s => s.Animals).FirstOrDefault(a => a.Id == animalId);
        }

        public async Task<List<Animal>> CreateAnimals(string userId, string spaceId, AnimalCreateDto dto)
        {
            if (!Constants.AllowedSpecies.Contains(dto.Species))
                throw new BadRequestException("Especie no permitida.");

            if (!Constants.AllowedBreedsBySpecies[dto.Species].Contains(dto.Breed))
                throw new BadRequestException("Raza no permitida para esa especie.");

            var space = await _context.Spaces.Find(s => s.Id == spaceId && s.UserId == userId).FirstOrDefaultAsync();
            if (space == null)
                throw new NotFoundException("Espacio no encontrado o no autorizado");

            var animal = space.Animals.FirstOrDefault(a => a.Species == dto.Species);
            if (animal == null)
            {
                var newAnimal = new Animal
                {
                    Species = dto.Species,
                    Breeds = new List<Animal.BreedQuantity>
                    {
                        new Animal.BreedQuantity { Breed = dto.Breed, Quantity = dto.Quantity }
                    }
                };
                space.Animals.Add(newAnimal);
            }
            else
            {
                var breed = animal.Breeds.FirstOrDefault(b => b.Breed == dto.Breed);
                if (breed == null)
                {
                    animal.Breeds.Add(new Animal.BreedQuantity { Breed = dto.Breed, Quantity = dto.Quantity });
                }
                else
                {
                    breed.Quantity += dto.Quantity;
                }
            }

            await _context.Spaces.ReplaceOneAsync(s => s.Id == spaceId && s.UserId == userId, space);
            return space.Animals;
        }

        public async Task<Animal> UpdateAnimal(string userId, string animalId, AnimalUpdateDto dto)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            var space = spaces.FirstOrDefault(s => s.Animals.Any(a => a.Id == animalId));
            if (space == null)
                throw new NotFoundException("Animal o espacio no encontrado o no autorizado");

            var animal = space.Animals.First(a => a.Id == animalId);

            if (!Constants.AllowedSpecies.Contains(dto.Species))
                throw new BadRequestException("Especie no permitida.");

            if (!Constants.AllowedBreedsBySpecies[dto.Species].Contains(dto.Breed))
                throw new BadRequestException("Raza no permitida para esa especie.");

            if (animal.Species != dto.Species)
            {
                animal.Species = dto.Species;
                animal.Breeds.Clear();
                animal.Breeds.Add(new Animal.BreedQuantity { Breed = dto.Breed, Quantity = dto.Quantity });
            }
            else
            {
                var breed = animal.Breeds.FirstOrDefault(b => b.Breed == dto.Breed);
                if (breed == null)
                {
                    animal.Breeds.Add(new Animal.BreedQuantity { Breed = dto.Breed, Quantity = dto.Quantity });
                }
                else
                {
                    breed.Quantity = dto.Quantity;
                }
            }

            await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            return animal;
        }

        public async Task<bool> DeleteAnimal(string userId, string animalId)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            var space = spaces.FirstOrDefault(s => s.Animals.Any(a => a.Id == animalId));
            if (space == null)
                throw new NotFoundException("Animal o espacio no encontrado o no autorizado");

            var animal = space.Animals.First(a => a.Id == animalId);
            space.Animals.Remove(animal);

            var result = await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<Animal> UpdateBreedName(string userId, string animalId, string oldBreedName, string newBreedName)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            var space = spaces.FirstOrDefault(s => s.Animals.Any(a => a.Id == animalId));
            if (space == null)
                throw new NotFoundException("Animal o espacio no encontrado o no autorizado");

            var animal = space.Animals.First(a => a.Id == animalId);
            var breed = animal.Breeds.FirstOrDefault(b => b.Breed == oldBreedName);
            if (breed == null)
                throw new NotFoundException("Raza no encontrada en este animal");

            if (animal.Breeds.Any(b => b.Breed == newBreedName))
                throw new BadRequestException("Ya existe una raza con ese nombre en este animal");

            if (!Constants.AllowedBreedsBySpecies[animal.Species].Contains(newBreedName))
                throw new BadRequestException("Raza no permitida para esta especie");

            breed.Breed = newBreedName;

            await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            return animal;
        }

        public async Task<Animal> DeleteBreed(string userId, string animalId, string breedName)
        {
            var spaces = await _context.Spaces.Find(s => s.UserId == userId).ToListAsync();
            var space = spaces.FirstOrDefault(s => s.Animals.Any(a => a.Id == animalId));
            if (space == null)
                throw new NotFoundException("Animal o espacio no encontrado o no autorizado");

            var animal = space.Animals.First(a => a.Id == animalId);
            var breed = animal.Breeds.FirstOrDefault(b => b.Breed == breedName);
            if (breed == null)
                throw new NotFoundException("Raza no encontrada en este animal");

            animal.Breeds.Remove(breed);

            await _context.Spaces.ReplaceOneAsync(s => s.Id == space.Id && s.UserId == userId, space);
            return animal;
        }
    }
}