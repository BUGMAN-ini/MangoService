using Mango.Services.Email.API.Data;
using Mango.Services.Email.API.Models;
using Mango.Services.Email.API.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Email.API.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> options;

        public EmailService(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }

        public Task EmailCartAndLog(CartDto email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var _db = new AppDbContext(options);
                await _db.EmailLogs.AddAsync(emailLog);
                await _db.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "RegistrationSuccesfull";
            await LogAndEmail(message, email);
        }
    }
}
