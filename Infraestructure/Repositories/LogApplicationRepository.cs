using Core.Entities;
using Core.Interfaces.Repository;
using Persistence.Data;

namespace Infraestructure.Repositories
{
    public class LogApplicationRepository : BaseRepository<LogApplication>, ILogApplicationRepository
    {
        public LogApplicationRepository(BaseDeDatosContext contex) : base(contex)
        {
        }
    }
}
