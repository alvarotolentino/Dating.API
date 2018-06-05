using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dating.API.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}