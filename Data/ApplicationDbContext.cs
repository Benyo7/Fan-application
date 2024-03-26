using Microsoft.EntityFrameworkCore;
using Fan_platform.Models;

namespace Fan_platform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Creation> NpuCreations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Score> Scores { get; set; }

        
    }
}

