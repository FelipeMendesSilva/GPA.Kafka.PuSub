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
        public ConfigurationModel Config { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public void OnGet()
        {

        }

        public void OnPost()
        {

        }

        public void OnPostPublishAsync(CancellationToken stoppingToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = Config.BootstrapServers,
                ClientId = Config.ClientId
            };


            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var t = producer.ProduceAsync("topic", new Message<Null, string> { Value = Message }, stoppingToken);
                t.ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        //msg nao enviada
                    }
                    else
                    {
                        //mensagem enviada
                    }
                });
                producer.Flush(TimeSpan.FromSeconds(5));
                producer.Dispose();
            }

        }
    }
}
