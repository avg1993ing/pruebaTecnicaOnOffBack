using Core.Entities;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class ValidarEmailService : BaseService<Users, ValidarEmailService>, IValidarEmailService
    {
        public ValidarEmailService(IAdminInterfaces adminInterfaces, ILogger<ValidarEmailService> logger) : base(adminInterfaces, logger)
        {
        }

        public async Task<bool> ValidateEmail(string token)
        {
            int userId = _adminInterfaces.utilsFunctionsRepository.GetIdUserToken($"Bearer {token}");
            var user = await _adminInterfaces.usersRepository.GetById(userId);
            user.IsActive = true; 
            await _adminInterfaces.usersRepository.UpdateAsync(user);
            return true;
        }
    }
}
