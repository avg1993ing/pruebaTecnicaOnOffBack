using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IValidarEmailService
    {
        Task<bool> ValidateEmail(string token);
    }
}
