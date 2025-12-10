using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ITaskUserService : IBaseService<TaskUser>
    {
        Task<TaskUser> Create(TaskUser entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<TaskUser>> GetAll();
        Task<TaskUser> GetById(int id);
        Task<TaskUser> Update(TaskUser entity);
        Task<IEnumerable<TaskUser>> GetAllTasksByIdUser(int id);
        Task<IEnumerable<TaskUser>> GetAllTasksByIdUseFilter(int id, bool? estado);
    }
}
