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
        public string GroupId { get; set; }
        public bool EnableAutoCommit { get; set; }
        public bool EnableAutoOffsetStore { get; set; }
        public int AutoOffsetReset { get; set; } 
        public int Loop { get; set; } = 1;
    }
}
