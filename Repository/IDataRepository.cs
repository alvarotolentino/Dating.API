using System.Collections.Generic;
using System.Threading.Tasks;
using Dating.API.Models;

namespace Dating.API.Repository
{
    public interface IDataRepository
    {
        IEnumerable<Value> Get(int? id = null);
        Task<IEnumerable<Value>> GetAsync(int? id = null);
    }
}