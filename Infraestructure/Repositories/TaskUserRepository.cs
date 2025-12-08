using Core.Entities;
using Core.Interfaces.Repository;
using Persistence.Data;

namespace Infraestructure.Repositories
{
    public class TaskUserRepository : BaseRepository<TaskUser>, ITaskUserRepository
    {
        public TaskUserRepository(BaseDeDatosContext contex) : base(contex)
        {
        }
    }
}
