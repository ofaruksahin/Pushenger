using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using StackExchange.Redis;
using System.Data;

namespace Pushenger.Service.Repositories
{
    public class RepositoryBase
    {
        private IDbTransaction _transaction { get; set; }

        private IDbContext _instance { get; set; }

        private ConnectionMultiplexer _cacheInstance { get; set; }

        protected IDbContext _connection
        {
            get
            {
                return _instance ?? (_instance = new DbContext(_transaction));
            }
        }

        protected ConnectionMultiplexer _cache
        {
            get
            {
                if (_cacheInstance == null)
                {
                    Connections connections = Connections.Instance;
                    //_cacheInstance = ConnectionMultiplexer.Connect(connections.RedisConnectionString);
                }
                return _cacheInstance;
            }
        }

        public RepositoryBase(IDbTransaction transaction)
        {
            _transaction = transaction;
        }
    }
}
