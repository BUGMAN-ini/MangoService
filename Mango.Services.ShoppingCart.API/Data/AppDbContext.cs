using Mango.Services.ShoppingCart.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCart.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }

    }
}
