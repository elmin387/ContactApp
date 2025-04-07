using System.Data;

namespace ContactApp.Infrastructure.Data
{
    public interface IConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}
