using System.Data;

namespace Dating.API.Repository
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}