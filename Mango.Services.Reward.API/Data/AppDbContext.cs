using Mango.Services.Reward.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Reward.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Rewards> Rewards { get; set; }

    }
}
