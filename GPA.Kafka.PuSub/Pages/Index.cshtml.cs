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
        public ConsumerConfigModel ConsumerConfig { get; set; }

        public void OnGet()
        {

        }

        public void OnPost()
        {

        }

        public IActionResult OnPostPublishAsync(CancellationToken stoppingToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = PublishConfig.BootstrapServers,
                ClientId = PublishConfig.ClientId
            };


            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                IActionResult result = new OkResult();
                var t = producer.ProduceAsync("topic", new Message<Null, string> { Value = PublishConfig.Payload }, stoppingToken);
                t.ContinueWith(task =>
                {

                    if (task.IsFaulted)
                    {
                        result = BadRequest();
                        //msg nao enviada
                    }
                });
                producer.Flush(TimeSpan.FromSeconds(5));
                producer.Dispose();
                return result;
            }

        }

        public IActionResult OnPostConsumeAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = ConsumerConfig.BootstrapServers,
                GroupId = ConsumerConfig.ClientId,
                EnableAutoCommit = ConsumerConfig.EnableAutoCommit,
                EnableAutoOffsetStore = ConsumerConfig.EnableAutoOffsetStore
            };


            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(ConsumerConfig.Topic);               
                var consumeResult = consumer.Consume(stoppingToken);
                consumer.Close();
                ConsumerConfig.Message = "Oi";
                return new OkObjectResult(consumeResult);
                
            }
        }
    }
}
