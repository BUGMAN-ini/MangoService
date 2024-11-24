using Mango.Services.Email.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Email.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<EmailLogger> EmailLogs { get; set; }



    }
}
