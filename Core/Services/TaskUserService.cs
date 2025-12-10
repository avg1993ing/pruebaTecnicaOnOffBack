using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class TaskUserService : BaseService<TaskUser, TaskUserService>, ITaskUserService
    {
        public TaskUserService(IAdminInterfaces adminInterfaces, ILogger<TaskUserService> logger) : base(adminInterfaces, logger)
        {
        }

        public async Task<TaskUser> Create(TaskUser entity)
        {
            return await _adminInterfaces.taskUserRepository.Add(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _adminInterfaces.taskUserRepository.GetById(id);
            if (entity == null) return false;

            return await _adminInterfaces.taskUserRepository.Delete(entity);
        }

        public async Task<IEnumerable<TaskUser>> GetAll()
        {
            return await _adminInterfaces.taskUserRepository.GetAll();
        }

        public async Task<IEnumerable<TaskUser>> GetAllTasksByIdUseFilter(int id, bool? estado)
        {
            return await _adminInterfaces.taskUserRepository.GetAllTasksByIdUseFilter(id, estado);
        }

        public async Task<IEnumerable<TaskUser>> GetAllTasksByIdUser(int id)
        {
            return await _adminInterfaces.taskUserRepository.GetAllTasksByIdUser(id);
        }

        public async Task<TaskUser> GetById(int id)
        {
            return await _adminInterfaces.taskUserRepository.GetById(id);
        }

        public async Task<TaskUser> Update(TaskUser entity)
        {
            return await _adminInterfaces.taskUserRepository.UpdateAsync(entity);
        }
    }
}
