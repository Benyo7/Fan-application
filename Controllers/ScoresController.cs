using Fan_platform.Data.Repositories;
using Fan_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fan_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly IScoreRepository _scoreRepository;

        public ScoresController(IScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        // POST: api/scores
        [HttpPost]
        public async Task<IActionResult> AddScore([FromBody] Score score)
        {
            // Validate score data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if the user has already scored this creation
            var existingScore = await _scoreRepository.GetScoreByUserAndCreationAsync(score);
            if (existingScore = true)
            {
                return BadRequest("You have already scored this creation.");
            }
           
            // Add the score
            await _scoreRepository.AddScoreAsync(score);

            return Ok("Score submitted successfully.");
        }

        // GET: api/scores/creations/{npuCreationId}
        [HttpGet("creations/{npuCreationId}")]
        public async Task<IActionResult> GetScoresForCreation(int npuCreationId)
        {
            var scores = await _scoreRepository.GetScoresForCreationAsync(npuCreationId);
            return Ok(scores);
        }

        // GET: api/scores/creations/{npuCreationId}/average
        [HttpGet("creations/{npuCreationId}/average")]
        public async Task<IActionResult> CalculateAverageScore(int npuCreationId)
        {
            var averageUniquenessScore = await _scoreRepository.CalculateAverageUniquenessScoreAsync(npuCreationId);
            var averageCreativityScore = await _scoreRepository.CalculateAverageCreativityScoreAsync(npuCreationId);
            // Update the average scores for the creation using ScoreRepository
            var updateResult = await _scoreRepository.UpdateAverageScoresForCreationAsync(npuCreationId, averageUniquenessScore, averageCreativityScore);

            if (updateResult)
            {
                return Ok(new
                {
                    AverageUniquenessScore = averageUniquenessScore,
                    AverageCreativityScore = averageCreativityScore
                });
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the creation.");
            }
        }

    }


}

