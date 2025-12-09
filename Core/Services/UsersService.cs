using Core.Entities;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class UsersService : BaseService<Users, UsersService>, IUsersService
    {
        public UsersService(IAdminInterfaces adminInterfaces, ILogger<UsersService> logger) : base(adminInterfaces, logger)
        {
        }

        public async Task<Users> Create(Users entity)
        {
            entity.IsActive = false;
            var user = await _adminInterfaces.usersRepository.Add(entity);
            if (user != null)
            {
                await _adminInterfaces.utilsFunctionsRepository.SMTP(
                    entity.Email,
                    "Activar cuenta",
                    await _adminInterfaces.utilsFunctionsRepository.GenerateTokenJWTRecoveryPassword(user));
            }

            return user;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _adminInterfaces.usersRepository.GetById(id);
            if (entity == null) return false;

            return await _adminInterfaces.usersRepository.Delete(entity);
        }

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _adminInterfaces.usersRepository.GetAll();
        }

        public async Task<Users> GetById(int id)
        {
            return await _adminInterfaces.usersRepository.GetById(id);
        }

        public async Task<Users> Update(Users entity)
        {
            return await _adminInterfaces.usersRepository.UpdateAsync(entity);
        }
    }
}
