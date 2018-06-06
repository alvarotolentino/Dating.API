using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dating.API.Repository
{
    public interface IGenericRepository<TEntity> where TEntity: class
    {
        Task<TEntity> Get(long Id);
        Task<IEnumerable<TEntity>> GetAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}