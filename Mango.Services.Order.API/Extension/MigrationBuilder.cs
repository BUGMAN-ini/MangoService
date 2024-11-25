using Mango.Services.Order.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Order.API.Extension
{
    public static class MigrationBuilder
    {
        public static void ApplyMigration(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
        }
    }
}
