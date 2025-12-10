using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface ITaskUserRepository : IBaseRepository<TaskUser>
    {
        Task<IEnumerable<TaskUser>> GetAllTasksByIdUser(int id);
        Task<IEnumerable<TaskUser>> GetAllTasksByIdUseFilter(int id, bool? estado);
    }
}
