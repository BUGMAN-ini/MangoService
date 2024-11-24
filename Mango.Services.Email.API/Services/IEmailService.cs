using Mango.Services.Email.API.Models.Dto;

namespace Mango.Services.Email.API.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto email);
        Task RegisterUserEmailAndLog(string email);
    }
}
