using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPA.Kafka.PuSub.Models
{
    public class PublishConfigModel
    {
        public string BootstrapServers { get; set; }
        public string ClientId { get; set; }
        public string Payload { get; set; }
        public string Topic { get; set; }
    }
}
