using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface IUsersRepository : IBaseRepository<Users>
    {
        Task<Users> GetByEmail(string name);
    }
}
