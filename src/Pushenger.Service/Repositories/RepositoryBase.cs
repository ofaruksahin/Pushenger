using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using StackExchange.Redis;
using System.Data;

namespace Pushenger.Service.Repositories
{
    public class RepositoryBase
    {
        private IDbTransaction transaction { get; set; }

        private IDbContext instance { get; set; }

        private ConnectionMultiplexer cacheInstance { get; set; }

        protected IDbContext connection
        {
            get
            {
                return instance ?? (instance = new DbContext(transaction));
            }
        }

        protected ConnectionMultiplexer _cache
        {
            get
            {
                if (cacheInstance == null)
                {
                    Connections connections = Connections.Instance;
                    //_cacheInstance = ConnectionMultiplexer.Connect(connections.RedisConnectionString);
                }
                return cacheInstance;
            }
        }

        public RepositoryBase(IDbTransaction transaction)
        {
            this.transaction = transaction;
        }
    }
}
