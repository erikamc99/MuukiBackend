using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Services.Interfaces;
using Muuki.DTOs;
using System.Security.Claims;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/animals")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue("id") ?? throw new Exception("Usuario no autenticado");
        }

        [HttpPost("{spaceId}")]
        public async Task<IActionResult> CreateAnimals(string spaceId, AnimalCreateDto dto)
        {
            var animals = await _animalService.CreateAnimals(GetUserId(), spaceId, dto);
            return Ok(animals);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            var animals = await _animalService.GetAllAnimals(GetUserId());
            var response = animals.Select(a => new AnimalResponseDto
            {
                Species = a.Species,
                Breeds = a.Breeds,
                Total = a.Breeds.Sum(b => b.Quantity)
            }).ToList();
            return Ok(response);
        }

        [HttpGet("{animalId}")]
        public async Task<IActionResult> GetAnimalById(string animalId)
        {
            var animal = await _animalService.GetAnimalById(GetUserId(), animalId);
            if (animal == null) return NotFound();
            var response = new AnimalResponseDto
            {
                Species = animal.Species,
                Breeds = animal.Breeds,
                Total = animal.Breeds.Sum(b => b.Quantity)
            };
            return Ok(response);
        }

        [HttpPut("{animalId}")]
        public async Task<IActionResult> UpdateAnimal(string animalId, AnimalUpdateDto dto)
        {
            var updated = await _animalService.UpdateAnimal(GetUserId(), animalId, dto);
            return Ok(updated);
        }

        [HttpDelete("{animalId}")]
        public async Task<IActionResult> DeleteAnimal(string animalId)
        {
            await _animalService.DeleteAnimal(GetUserId(), animalId);
            return Ok("Animal eliminado correctamente");
        }

        [HttpPut("{animalId}/breed")]
        public async Task<IActionResult> UpdateBreedName(string animalId, [FromBody] BreedDto dto)
        {
            var updated = await _animalService.UpdateBreedName(GetUserId(), animalId, dto.OldBreedName, dto.NewBreedName);
            return Ok(updated);
        }

        [HttpDelete("{animalId}/breed/{breedName}")]
        public async Task<IActionResult> DeleteBreed(string animalId, string breedName)
        {
            var updated = await _animalService.DeleteBreed(GetUserId(), animalId, breedName);
            return Ok(updated);
        }
    }
}