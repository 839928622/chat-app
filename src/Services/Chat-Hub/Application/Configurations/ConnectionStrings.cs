using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configurations
{
    public class ConnectionStrings
    {
        public string ChatApp { get; set; } = null!;
        public string RedisConnectionString { get; set; } = null!;
        public string RedisConnectionStringForScalingSignalR { get; set; } = null!;
        public string EventBusHostAddress { get; set; } = null!;
    }
}
