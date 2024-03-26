using Fan_platform.Data;
using Fan_platform.Models;
using Microsoft.EntityFrameworkCore;


namespace Fan_platform.Data.Repositories
{
    public class NpuCreationRepository : INpuCreationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NpuCreationRepository> _logger;

        public NpuCreationRepository(ApplicationDbContext context, ILogger<NpuCreationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Creation> GetByIdAsync(int id)
        {
            try
            {
                return await _context.NpuCreations.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving NPU creation by ID.");
                throw; // Re-throw the exception to be handled at a higher level
            }
        }

        public async Task<List<Creation>> GetAllAsync()
        {
            try
            {
                return await _context.NpuCreations.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all NPU creations.");
                throw;
            }
        }

        public async Task AddAsync(Creation npuCreation)
        {
            try
            {
                _context.NpuCreations.Add(npuCreation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding an NPU creation.");
                throw;
            }
        }

        public async Task UpdateAsync(Creation npuCreation)
        {
            try
            {
                _context.Entry(npuCreation).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating an NPU creation.");
                throw;
            }
        }

        public async Task DeleteAsync(Creation npuCreation)
        {
            try
            {
                _context.NpuCreations.Remove(npuCreation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting an NPU creation.");
                throw;
            }
        }
        public async Task<List<Creation>> SearchByNameAsync(string name)
        {
            return await _context.NpuCreations.Where(c => c.Name.Contains(name)).ToListAsync();
        }
    }



    public interface INpuCreationRepository
    {
        Task<Creation> GetByIdAsync(int id);
        Task<List<Creation>> GetAllAsync();
        Task AddAsync(Creation npuCreation);
        Task UpdateAsync(Creation npuCreation);
        Task DeleteAsync(Creation npuCreation);
        Task<List<Creation>> SearchByNameAsync(string name);
    }
}

