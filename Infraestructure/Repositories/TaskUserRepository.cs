using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Infraestructure.Repositories
{
    public class TaskUserRepository : BaseRepository<TaskUser>, ITaskUserRepository
    {
        public TaskUserRepository(BaseDeDatosContext contex) : base(contex)
        {
        }

        public async Task<IEnumerable<TaskUser>> GetAllTasksByIdUseFilter(int id, bool? estado)
        {
            if(estado != null)
            {
                return await _entities
                .Where(t => t.idUsers == id && t.Complete == estado)
                .ToArrayAsync();
            }
            else
            {
                return await _entities
                .Where(t => t.idUsers == id)
                .ToArrayAsync();
            }
        }

        public async Task<IEnumerable<TaskUser>> GetAllTasksByIdUser(int id)
        {
            return await _entities.Where(t => t.idUsers == id).ToArrayAsync();
        }
    }
}
