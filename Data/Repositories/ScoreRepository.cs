using Fan_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Fan_platform.Data.Repositories
{
    public interface IScoreRepository
    {
        Task AddScoreAsync(Score score);
        Task<List<Score>> GetScoresForCreationAsync(int npuCreationId);
        Task<double> CalculateAverageCreativityScoreAsync(int npuCreationId);
        Task<double> CalculateAverageUniquenessScoreAsync(int npuCreationId);
        Task<bool> UpdateAverageScoresForCreationAsync (int npuCreationId, double averageUniquenessScore, double averageCreativityScore);
        Task<bool> GetScoreByUserAndCreationAsync(Score score);
    }
   

    public class ScoreRepository : IScoreRepository
    {
        private readonly ApplicationDbContext _context;

        public ScoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddScoreAsync(Score score)
        {
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
        }

        public  async Task<bool> GetScoreByUserAndCreationAsync(Score score)
        {
            var existingscores = await _context.Scores.Where(s => (s.NpuCreationId == score.NpuCreationId)&& (s.UserId == score.UserId)).ToListAsync();
            if (existingscores != null)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<List<Score>> GetScoresForCreationAsync(int npuCreationId)
        {
            return await _context.Scores.Where(s => s.NpuCreationId == npuCreationId).ToListAsync();
        }

        public async Task<double> CalculateAverageCreativityScoreAsync(int npuCreationId)
        {
            var scores = await _context.Scores.Where(s => s.NpuCreationId == npuCreationId).ToListAsync();
            return scores.Average(s => s.CreativityScore);
        }

        public async Task<double> CalculateAverageUniquenessScoreAsync(int npuCreationId)
        {
            var scores = await _context.Scores.Where(s => s.NpuCreationId == npuCreationId).ToListAsync();
            return scores.Average(s => s.UniquenessScore);
        }
        public async Task<bool> UpdateAverageScoresForCreationAsync(int npuCreationId, double averageUniquenessScore, double averageCreativityScore)
        {
            var creation = await _context.NpuCreations.FindAsync(npuCreationId);

            if (creation == null)
            {
                return false; 
            }

            creation.AVGUniqueScore = averageUniquenessScore;
            creation.AVGCreativeScore = averageCreativityScore;

            try
            {
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }

}

