using Core.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class BaseService<TEntity, TService> where TEntity : class
    {
        public readonly ILogger<TService> _logger;
        public readonly IAdminInterfaces _adminInterfaces;

        public BaseService(IAdminInterfaces adminInterfaces, ILogger<TService> logger)
        {
            _adminInterfaces = adminInterfaces;
            _logger = logger;
        }
    }
}
