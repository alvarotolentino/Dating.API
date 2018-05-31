using System.Collections.Generic;
using Dating.API.Models;

namespace Dating.API.Repository
{
    public interface IDataRepository
    {
        Value RetriveById(int id);
        IEnumerable<Value> Get(int? id = null);
    }
}