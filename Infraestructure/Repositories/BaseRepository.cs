using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Infraestructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly BaseDeDatosContext _contex;
        protected readonly DbSet<T> _entities;
        public BaseRepository(BaseDeDatosContext contex)
        {
            _contex = contex;
            _entities = contex.Set<T>();
        }
        public async Task<T> Add(T entity)
        {
            try
            {
                var result = await _entities.AddAsync(entity);
                await _contex.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task AddRange(List<T> entity)
        {
            await _entities.AddRangeAsync(entity);
            await _contex.SaveChangesAsync();
        }

        public virtual async Task<bool> Delete(T entity)
        {
            try
            {
                _entities.Remove(entity);
                await _contex.SaveChangesAsync();
            }
            catch (NotSupportedException)
            {
                return false;
            }
            return true;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await _entities.Where(x => x.id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _contex.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateRangeAsync(List<T> entity)
        {
            _entities.UpdateRange(entity);
            await _contex.SaveChangesAsync();
        }
    }
}
