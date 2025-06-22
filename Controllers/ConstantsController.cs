using Microsoft.AspNetCore.Mvc;
using Muuki.Models;

namespace Muuki.Controllers
{
    [ApiController]
    [Route("api/constants")]
    public class ConstantsController : ControllerBase
    {
        [HttpGet("space-types")]
        public IActionResult GetSpaceTypes()
        {
            return Ok(Constants.AllowedSpaceTypes);
        }

        [HttpGet("animal-types")]
        public IActionResult GetAnimalTypes()
        {
            return Ok(Constants.AllowedSpecies);
        }

        [HttpGet("breeds")]
        public IActionResult GetBreeds()
        {
            return Ok(Constants.AllowedBreedsBySpecies);
        }
    }
}