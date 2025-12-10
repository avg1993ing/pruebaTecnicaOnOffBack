using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Infraestructure.Repositories
{
    public class UsersRepository : BaseRepository<Users>, IUsersRepository
    {
        public UsersRepository(BaseDeDatosContext contex) : base(contex)
        {
        }

        public async Task<Users> GetByEmail(string email)
        {
            return await _entities.FirstOrDefaultAsync(u => u.Email == email && u.IsActive == true);
        }
    }
}
