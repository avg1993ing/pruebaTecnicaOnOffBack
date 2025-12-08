using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data
{
    public class BaseDeDatosContext : DbContext
    {
        public BaseDeDatosContext(DbContextOptions<BaseDeDatosContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<TaskUser> TaskUser { get; set; }
        public virtual DbSet<LogApplication> LogApplication { get; set; }
    }
}
