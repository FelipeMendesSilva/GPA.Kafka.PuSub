using Confluent.Kafka;
using GPA.Kafka.PuSub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GPA.Kafka.PuSub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

        }

        [BindProperty]
        public PublishConfigModel PublishConfig { get; set; }
        [BindProperty]
        public ConsumerConfigModel ConsumeConfig { get; set; }
        
        public void OnGet()
        {

        }

        public void OnPost()
        {

        }

        public void OnPostPublishAsync(CancellationToken stoppingToken)
        {
            ViewData["Publish Result"] = "";
            ViewData["Fail Result"] = "";
            ViewData["Success Result"] = "";
            if (String.IsNullOrEmpty(PublishConfig.BootstrapServers)) { ViewData["Publish Result"] = "Error: BootstrapServes invalid parameter"; return; }
            if (String.IsNullOrEmpty(PublishConfig.ClientId)) { ViewData["Publish Result"] = "Error: GroupId invalid parameter"; return; }

            var config = new ProducerConfig
            {
                BootstrapServers = PublishConfig.BootstrapServers,
                ClientId = PublishConfig.ClientId
            };

            var result = true;
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var t = producer.ProduceAsync("topic", new Message<Null, string> { Value = PublishConfig.Payload }, stoppingToken);
                t.ContinueWith(task =>
                {
                    if (!task.IsFaulted) {result = true;}
                    else {result = false;}
                });
                if (result)
                {
                    ViewData["Publish Result"] = "";
                    ViewData["Fail Result"] = "";
                    ViewData["Success Result"] = "Success";
                    //msg enviada
                }
                else
                {
                    ViewData["Publish Result"] = "";
                    ViewData["Success Result"] = "";
                    ViewData["Fail Result"] = "Fail";
                }
                producer.Flush(TimeSpan.FromSeconds(5));
                producer.Dispose();
                
            }
        }

        public void OnPostConsumeAsync(CancellationToken stoppingToken)
        {
            ViewData["Consume Result"] = "";
            if (String.IsNullOrEmpty(ConsumeConfig.BootstrapServers)) { ViewData["Consume Result"] = "Error: BootstrapServes invalid parameter"; return; }
            if (String.IsNullOrEmpty(ConsumeConfig.GroupId)) { ViewData["Consume Result"] = "Error: GroupId invalid parameter"; return; }
            if (String.IsNullOrEmpty(ConsumeConfig.Topic)) { ViewData["Consume Result"] = "Error: Topic invalid parameter"; return; }

            var config = new ConsumerConfig
            {
                BootstrapServers = ConsumeConfig.BootstrapServers,
                GroupId = ConsumeConfig.GroupId,
                EnableAutoCommit = ConsumeConfig.EnableAutoCommit,
                EnableAutoOffsetStore = ConsumeConfig.EnableAutoOffsetStore
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                var messages = "";
                consumer.Subscribe(ConsumeConfig.Topic);
                for (int i = 0; i < ConsumeConfig.Loop; i++)
                {
                    messages += "hi\n"; // consumer.Consume(stoppingToken);
                    ViewData["Messages"] = messages;
                }                
                consumer.Close();
                
            }
        }
    }
}
