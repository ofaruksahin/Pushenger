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

        IConfiguration _configuration;

        private Connections()
        {
            EnvironmentManager environmentManager = EnvironmentManager.Instance;
            _configuration = environmentManager.GetConfiguration();
        }
    }
}
