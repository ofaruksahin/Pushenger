using environment.net.core;
using Microsoft.Extensions.Configuration;

namespace Pushenger.Core.Utilities
{
    public class Connections
    {
        private static volatile Connections _connections;
        
        public static Connections Instance
        {
            get
            {
                if (_connections == null)
                    _connections = new Connections();
                return _connections;
            }
        }

        IConfiguration configuration;

        private Connections()
        {
            EnvironmentManager environmentManager = EnvironmentManager.Instance;
            configuration = environmentManager.GetConfiguration();
        }

        public string MysqlConnectionString => (string)configuration.GetValue(typeof(string), "mysql_connection");
    }
}
