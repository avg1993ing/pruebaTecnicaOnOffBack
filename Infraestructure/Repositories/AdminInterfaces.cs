using Core.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Persistence.Data;

namespace Infraestructure.Repositories
{
    public class AdminInterfaces : IAdminInterfaces
    {
        public BaseDeDatosContext _context;
        public IConfiguration _configuration;
        public AdminInterfaces(BaseDeDatosContext context,
                               IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private readonly IUsersRepository _usersRepository;
        private readonly ITaskUserRepository _taskUserRepository;
        private readonly ILogApplicationRepository _logApplicationRepository;
        private readonly IUtilsFunctionsRepository _utilsFunctionsRepository;

        public IUsersRepository usersRepository => _usersRepository ?? new UsersRepository(_context);
        public ITaskUserRepository taskUserRepository => _taskUserRepository ?? new TaskUserRepository(_context);
        public ILogApplicationRepository logApplicationRepository => _logApplicationRepository ?? new LogApplicationRepository(_context);
        public IUtilsFunctionsRepository utilsFunctionsRepository => _utilsFunctionsRepository ?? new UtilsFunctionsRepository(_configuration);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
