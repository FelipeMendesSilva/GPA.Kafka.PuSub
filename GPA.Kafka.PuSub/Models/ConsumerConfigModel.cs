using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPA.Kafka.PuSub.Models
{
    public class ConsumerConfigModel
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
        public string ClientId { get; set; }
        public bool EnableAutoCommit { get; set; }
        public bool EnableAutoOffsetStore { get; set; }
        public string AutoOffsetReset { get; set; }        
        public string Message { get; set; }
    }
}
