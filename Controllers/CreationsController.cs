using Fan_platform.Data.Repositories;
using Fan_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fan_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NpuCreationsController : ControllerBase
    {
        private readonly INpuCreationRepository _npuCreationRepository;

        public NpuCreationsController(INpuCreationRepository npuCreationRepository)
        {
            _npuCreationRepository = npuCreationRepository;
        }

        // GET: api/NpuCreations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Creation>>> GetNpuCreations()
        {
            return await _npuCreationRepository.GetAllAsync();
        }

        // GET: api/NpuCreations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Creation>> GetNpuCreation(int id)
        {
            var npuCreation = await _npuCreationRepository.GetByIdAsync(id);

            if (npuCreation == null)
            {
                return NotFound();
            }

            return npuCreation;
        }

        // POST: api/NpuCreations
        [HttpPost]
        [Authorize] // Requires authentication to access this endpoint
        public async Task<ActionResult<Creation>> PostNpuCreation(Creation npuCreation)
        {
            try
            {
                await _npuCreationRepository.AddAsync(npuCreation);
                return CreatedAtAction(nameof(GetNpuCreation), new { id = npuCreation.Id }, npuCreation);
            }
            catch (Exception)
            {
                // Log and handle the exception
                return StatusCode(500, "An error occurred while creating the NPU creation.");
            }
        }

        // PUT: api/NpuCreations/5
        [HttpPut("{id}")]
        [Authorize] // Requires authentication to access this endpoint
        public async Task<IActionResult> PutNpuCreation(int id,Creation npuCreation)
        {
            if (id != npuCreation.Id)
            {
                return BadRequest();
            }

            try
            {
                await _npuCreationRepository.UpdateAsync(npuCreation);
            }
            catch (Exception)
            {
                // Log and handle the exception
                return StatusCode(500, "An error occurred while updating the NPU creation.");
            }

            return NoContent();
        }

        // DELETE: api/NpuCreations/5
        [HttpDelete("{id}")]
        [Authorize] // Requires authentication to access this endpoint
        public async Task<IActionResult> DeleteNpuCreation(int id)
        {
            var npuCreation = await _npuCreationRepository.GetByIdAsync(id);
            if (npuCreation == null)
            {
                return NotFound();
            }

            try
            {
                await _npuCreationRepository.DeleteAsync(npuCreation);
            }
            catch (Exception)
            {
                // Log and handle the exception
                return StatusCode(500, "An error occurred while deleting the NPU creation.");
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCreations(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }

            var creations = await _npuCreationRepository.SearchByNameAsync(name);

            return Ok(creations);
        }
    }
}
